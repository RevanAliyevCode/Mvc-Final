using System;
using E = Domain.Entities;

namespace Business.Services.Cart;

public interface ICartService
{
    Task<E.Basket?> GetBasketVMAsync();
}
