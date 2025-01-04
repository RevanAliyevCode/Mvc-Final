using System;
using Data.Repositories.Basket;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using E = Domain.Entities;

namespace Business.Services.Cart;

public class CartService : ICartService
{
    readonly IBasketRepo _basketRepo;
    readonly UserManager<AppUser> _userManager;
    readonly IActionContextAccessor _accessor;

    public CartService(IBasketRepo basketRepo, UserManager<AppUser> userManager, IActionContextAccessor accessor)
    {
        _basketRepo = basketRepo;
        _userManager = userManager;
        _accessor = accessor;
    }

    public async Task<E.Basket?> GetBasketVMAsync()
    {
        var user = await _userManager.GetUserAsync(_accessor.ActionContext.HttpContext.User);

        if (user == null)
            return null;

        var basket = await _basketRepo.GetBasketByUserIdWithItemsAndProuctsAsync(user.Id);

        if (basket == null)
            return null;

        return basket;
    }
}
