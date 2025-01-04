using System;
using E = Domain.Entities;
using Data.Repositories.Base;
using Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Basket;

public class BasketRepo : BaseRepo<E.Basket>, IBasketRepo
{
    readonly AppDbContext _context;
    public BasketRepo(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<E.Basket?> GetBasketByUserIdAsync(string userId)
    {
        return await _context.Baskets.FirstOrDefaultAsync(x => x.UserId == userId);
    }

    public async Task<E.Basket?> GetBasketByUserIdWithItemsAsync(string userId)
    {
        return await _context.Baskets.Include(b => b.Items).FirstOrDefaultAsync(x => x.UserId == userId);
    }

    public Task<E.Basket?> GetBasketByUserIdWithItemsAndProuctsAsync(string userId)
    {
        return _context.Baskets.Include(b => b.Items).ThenInclude(bi => bi.Product).FirstOrDefaultAsync(x => x.UserId == userId);
    }
}
