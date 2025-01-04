using System;
using Data.Repositories.Base;
using E = Domain.Entities;

namespace Data.Repositories.Order;

public interface IOrderRepo : IBaseRepository<E.Order>
{
    Task<E.Order?> GetOrderByTrackIdWithItemsAndProduct(Guid trackId);
}
