using System;
using Data.Repositories.Base;
using E = Domain.Entities;

namespace Data.Repositories.Basket;

public interface IBasketRepo : IBaseRepository<E.Basket>
{
    Task<E.Basket?> GetBasketByUserIdAsync(string userId);
    Task<E.Basket?> GetBasketByUserIdWithItemsAsync(string userId);
    Task<E.Basket?> GetBasketByUserIdWithItemsAndProuctsAsync(string userId);
}
