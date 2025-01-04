using System;
using Data.Repositories.Base;
using E = Domain.Entities;

namespace Data.Repositories.BasketItem;

public interface IBasketItemRepo : IBaseRepository<E.BasketItem>
{
    Task<E.BasketItem?> GetBasketItemByBasketIdAndProductIdAsync(int basketId, int productId);
}
