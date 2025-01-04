using System;

namespace Business.Services.Basket;

public interface IBasketService
{
    Task<(int code, string message)> AddToBasketAsync(int productId);
    Task<(int code, string message)> RemoveFromBasketAsync(int basketItemId);
    Task<(int code, string message)> IncrementProductCountAsync(int basketItemId);
    Task<(int code, string message)> DecrementProductCountAsync(int basketItemId);
}
