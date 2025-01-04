using System;
using Business.Services.Basket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fruitkha.Controllers;

[Authorize(Roles = "Seller, Customer")]
public class BasketController : Controller
{
    readonly IBasketService _basketService;

    public BasketController(IBasketService basketService)
    {
        _basketService = basketService;
    }

    [HttpPost]
    public async Task<IActionResult> AddToBasket(int productId)
    {
        var (code, message) = await _basketService.AddToBasketAsync(productId);

        if (code == 200)
            return Ok(message);

        return code switch
        {
            401 => Unauthorized(message),
            404 => NotFound(message),
            400 => BadRequest(message),
            _ => BadRequest("Something went wrong")
        };
    }

    [HttpPost]
    public async Task<IActionResult> RemoveFromBasket(int basketItemId)
    {
        var (code, message) = await _basketService.RemoveFromBasketAsync(basketItemId);

        if (code == 200)
            return Ok(message);

        return code switch
        {
            401 => Unauthorized(message),
            404 => NotFound(message),
            400 => BadRequest(message),
            _ => BadRequest("Something went wrong")
        };
    }

    [HttpPost]
    public async Task<IActionResult> IncrementItem(int basketItemId)
    {
        var (code, message) = await _basketService.IncrementProductCountAsync(basketItemId);

        if (code == 200)
            return Ok(message);

        return code switch
        {
            401 => Unauthorized(message),
            404 => NotFound(message),
            400 => BadRequest(message),
            _ => BadRequest("Something went wrong")
        };
    }

    [HttpPost]
    public async Task<IActionResult> DecrementItem(int basketItemId)
    {
        var (code, message) = await _basketService.DecrementProductCountAsync(basketItemId);

        if (code == 200)
            return Ok(message);

        return code switch
        {
            401 => Unauthorized(message),
            404 => NotFound(message),
            400 => BadRequest(message),
            _ => BadRequest("Something went wrong")
        };
    }
    
}
