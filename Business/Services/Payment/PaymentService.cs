using System;
using Data.Repositories.Basket;
using Data.Repositories.Order;
using Data.Repositories.OrderItem;
using Data.UnitOfWork;
using Domain.Entities;
using Domain.Entities.Identity;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Stripe;
using Stripe.Checkout;

namespace Business.Services.Payment;

public class PaymentService : IPaymentService
{
    readonly UserManager<AppUser> _userManager;
    readonly IOrderRepo _orderRepo;
    readonly IOrderItemRepo _orderItemRepo;
    readonly IBasketRepo _basketRepo;
    readonly IActionContextAccessor _actionContextAccessor;
    readonly IUnitOfWork _unitOfWork;
    readonly LinkGenerator _linkGenerator;

    public PaymentService(UserManager<AppUser> userManager, IActionContextAccessor actionContextAccessor, IBasketRepo basketRepo, IOrderRepo orderRepo, LinkGenerator linkGenerator, IOrderItemRepo orderItemRepo, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _actionContextAccessor = actionContextAccessor;
        _basketRepo = basketRepo;
        _orderRepo = orderRepo;
        _linkGenerator = linkGenerator;
        _orderItemRepo = orderItemRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<(int code, string message)> CreateCheckoutSessionAsync()
    {
        var user = await _userManager.GetUserAsync(_actionContextAccessor.ActionContext.HttpContext.User);

        if (user == null)
            return (400, "User not found");

        var basket = await _basketRepo.GetBasketByUserIdWithItemsAndProuctsAsync(user.Id);

        if (basket == null)
            return (404, "Basket not found");

        var order = new Order
        {
            UserId = user.Id,
            Status = StatusEnum.Pending,
            TrackingNumber = Guid.NewGuid(),
        };

        await _orderRepo.AddAsync(order);

        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string>
            {
                "card",
            },
            Mode = "payment",
            SuccessUrl = _linkGenerator.GetUriByAction(_actionContextAccessor.ActionContext.HttpContext, "Success", "Payment", new { trackId = order.TrackingNumber }),
            CancelUrl = _linkGenerator.GetUriByAction(_actionContextAccessor.ActionContext.HttpContext, "Cancel", "Payment", new { trackId = order.TrackingNumber }),
        };

        var items = new List<SessionLineItemOptions>();

        if (basket.Items.Count == 0)
        {
            return (400, "Basket is empty");
        }

        foreach (var item in basket.Items)
        {
            var orderItem = new OrderItem
            {
                Order = order,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = item.Price,
            };

            if (item.Product.Stock < item.Quantity)
            {
                return (400, "Not enough stock");
            }

            await _orderItemRepo.AddAsync(orderItem);
            items.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = item.Product.Name,
                    },
                    UnitAmount = (long)(item.Product.Price * 100),
                },
                Quantity = item.Quantity,
            });

            item.Product.Stock -= item.Quantity;
        }

        items.Add(new SessionLineItemOptions
        {
            PriceData = new SessionLineItemPriceDataOptions
            {
                Currency = "usd",
                ProductData = new SessionLineItemPriceDataProductDataOptions
                {
                    Name = "Delivery Fee",
                },
                UnitAmount = 4500,
            },
            Quantity = 1,
        });

        options.LineItems = items;

        try
        {
            var service = new SessionService();
            Session session = await service.CreateAsync(options);
            await _unitOfWork.SaveChangesAsync();
            return (200, session.Id);
        }
        catch (StripeException e)
        {
            return (400, e.Message);
        }

    }

    public async Task<(int code, string message)> SuccessAsync(Guid trackId)
    {
        var user = await _userManager.GetUserAsync(_actionContextAccessor.ActionContext.HttpContext.User);
        if (user == null)
            return (401, "Unauthorize");

        var basket = await _basketRepo.GetBasketByUserIdWithItemsAndProuctsAsync(user.Id);

        if (basket == null)
            return (404, "Basket not found");

        var order = await _orderRepo.GetOrderByTrackIdWithItemsAndProduct(trackId);

        if (order == null)
            return (404, "Order not found");

        order.Status = StatusEnum.Processing;

        basket.Items.Clear();

        await _unitOfWork.SaveChangesAsync();
        return (200, "Success");
    }

    public async Task<(int code, string message)> CancelAsync(Guid trackId)
    {
        var user = await _userManager.GetUserAsync(_actionContextAccessor.ActionContext.HttpContext.User);
        if (user == null)
            return (401, "Unauthorize");

        var order = await _orderRepo.GetOrderByTrackIdWithItemsAndProduct(trackId);

        if (order == null)
            return (404, "Order not found");

        order.Status = StatusEnum.Processing;

        foreach (var item in order.Items)
        {
            item.Product.Stock += item.Quantity;
        }

        await _unitOfWork.SaveChangesAsync();
        return (200, "Success");
    }
}
