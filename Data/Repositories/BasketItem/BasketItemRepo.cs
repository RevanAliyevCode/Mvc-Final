using System;
using E = Domain.Entities;
using Data.Repositories.Base;
using Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.BasketItem;

public class BasketItemRepo : BaseRepo<E.BasketItem>, IBasketItemRepo
{
    readonly AppDbContext _context;
    public BasketItemRepo(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<E.BasketItem?> GetBasketItemByBasketIdAndProductIdAsync(int basketId, int productId)
    {
        return await _context.BasketItems.FirstOrDefaultAsync(x => x.BasketId == basketId && x.ProductId == productId);
    }
}
