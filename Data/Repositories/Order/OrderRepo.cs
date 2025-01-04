using System;
using Data.Contexts;
using Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using E = Domain.Entities;

namespace Data.Repositories.Order;

public class OrderRepo : BaseRepo<E.Order>, IOrderRepo
{
    readonly AppDbContext _appDbContext;
    public OrderRepo(AppDbContext context) : base(context)
    {
        _appDbContext = context;
    }

    public async Task<E.Order?> GetOrderByTrackIdWithItemsAndProduct(Guid trackId)
    {
        return await _appDbContext.Orders
            .Include(x => x.Items)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.TrackingNumber == trackId);
    }
}
