using System;
using Data.Contexts;
using Data.Repositories.Base;
using E = Domain.Entities;

namespace Data.Repositories.OrderItem;

public class OrderItemRepo : BaseRepo<E.OrderItem>, IOrderItemRepo
{
    public OrderItemRepo(AppDbContext context) : base(context)
    {
    }
}
