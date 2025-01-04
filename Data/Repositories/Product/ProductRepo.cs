using System;
using Data.Contexts;
using Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using E = Domain.Entities;

namespace Data.Repositories.Product;

public class ProductRepo : BaseRepo<E.Product>, IProductRepo
{
    readonly AppDbContext _context;
    public ProductRepo(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<E.Product>> GetAllProductWithCategoriesAsync()
    {
        return await _context.Products.Include(p => p.Category).ToListAsync();
    }

    public async Task<E.Product?> GetProductWithCategoriesAsync(int id)
    {
        return await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
    }
}
