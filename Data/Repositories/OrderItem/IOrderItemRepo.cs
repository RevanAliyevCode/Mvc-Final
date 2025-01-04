using System;
using Data.Repositories.Base;
using E = Domain.Entities;

namespace Data.Repositories.OrderItem;

public interface IOrderItemRepo : IBaseRepository<E.OrderItem>
{
}
