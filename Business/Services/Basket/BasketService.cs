using System;
using Data.Repositories.Basket;
using Data.Repositories.BasketItem;
using Data.Repositories.Product;
using Data.UnitOfWork;
using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Business.Services.Basket;

public class BasketService : IBasketService
{
    readonly UserManager<AppUser> _userManager;
    readonly IActionContextAccessor _accessor;
    readonly IBasketRepo _basketRepo;
    readonly IBasketItemRepo _basketItemRepo;
    readonly IProductRepo _productRepo;
    readonly IUnitOfWork _unitOfWork;

    public BasketService(UserManager<AppUser> userManager, IActionContextAccessor accessor, IBasketRepo basketRepo, IProductRepo productRepo, IUnitOfWork unitOfWork, IBasketItemRepo basketItemRepo)
    {
        _userManager = userManager;
        _accessor = accessor;
        _basketRepo = basketRepo;
        _productRepo = productRepo;
        _unitOfWork = unitOfWork;
        _basketItemRepo = basketItemRepo;
    }


    public async Task<(int code, string message)> AddToBasketAsync(int productId)
    {
        var user = await _userManager.GetUserAsync(_accessor.ActionContext.HttpContext.User);

        if (user == null)
            return (401, "Unauthorized");

        var basket = await _basketRepo.GetBasketByUserIdWithItemsAsync(user.Id);

        if (basket == null)
            return (404, "Basket not found");

        var product = await _productRepo.GetByIdAsync(productId);

        if (product == null)
            return (404, "Product not found");

        var basketItemFinded = await _basketItemRepo.GetBasketItemByBasketIdAndProductIdAsync(basket.Id, product.Id);

        if (basketItemFinded != null)
        {

            if (product.Stock < basketItemFinded.Quantity + 1)
            {
                return (400, "Stock is not enough");
            }

            basketItemFinded.Quantity++;
            await _unitOfWork.SaveChangesAsync();
            return (200, "Successfully added");
        }


        var basketItem = new BasketItem
        {
            BasketId = basket.Id,
            ProductId = product.Id,
            Quantity = 1,
            Price = product.Price,
        };

        await _basketItemRepo.AddAsync(basketItem);
        await _unitOfWork.SaveChangesAsync();

        return (200, "Successfully added");
    }

    public async Task<(int code, string message)> IncrementProductCountAsync(int basketItemId)
    {
        var user = await _userManager.GetUserAsync(_accessor.ActionContext.HttpContext.User);

        if (user == null)
        {
            return (401, "Unauthorized");
        }

        var basket = await _basketRepo.GetBasketByUserIdWithItemsAsync(user.Id);

        if (basket == null)
        {
            return (404, "Basket not found");
        }

        var basketItem = await _basketItemRepo.GetByIdAsync(basketItemId);

        if (basketItem == null)
        {
            return (404, "Basket item not found");
        }

        var product = await _productRepo.GetByIdAsync(basketItem.ProductId);

        if (product?.Stock < basketItem.Quantity + 1)
        {
            return (400, "Stock is not enough");
        }

        basketItem.Quantity++;
        await _unitOfWork.SaveChangesAsync();

        return (200, "Successfully incremented");
    }

    public async Task<(int code, string message)> DecrementProductCountAsync(int basketItemId)
    {
        var user = await _userManager.GetUserAsync(_accessor.ActionContext.HttpContext.User);

        if (user == null)
        {
            return (401, "Unauthorized");
        }

        var basket = await _basketRepo.GetBasketByUserIdWithItemsAsync(user.Id);

        if (basket == null)
        {
            return (404, "Basket not found");
        }

        var basketItem = await _basketItemRepo.GetByIdAsync(basketItemId);

        if (basketItem == null)
        {
            return (404, "Basket item not found");
        }

        if (basketItem.Quantity == 1)
        {
            _basketItemRepo.DeleteAsync(basketItem);
            await _unitOfWork.SaveChangesAsync();
            return (200, "Successfully removed");
        }

        basketItem.Quantity--;
        await _unitOfWork.SaveChangesAsync();

        return (200, "Successfully decremented");
    }

    public async Task<(int code, string message)> RemoveFromBasketAsync(int basketItemId)
    {
        var user = await _userManager.GetUserAsync(_accessor.ActionContext.HttpContext.User);

        if (user == null)
        {
            return (401, "Unauthorized");
        }

        var basket = await _basketRepo.GetBasketByUserIdWithItemsAsync(user.Id);

        if (basket == null)
        {
            return (404, "Basket not found");
        }

        var basketItem = await _basketItemRepo.GetByIdAsync(basketItemId);

        if (basketItem == null)
        {
            return (404, "Basket item not found");
        }

        _basketItemRepo.DeleteAsync(basketItem);
        await _unitOfWork.SaveChangesAsync();

        return (200, "Successfully removed");
    }
}
