using System;
using Data.Contexts;
using Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using E = Domain.Entities;

namespace Data.Repositories.Category;

public class CategoryRepo : BaseRepo<E.Category>, ICategoryRepo
{
    readonly AppDbContext _context;
    public CategoryRepo(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<E.Category?> GetCategoryByNameAsync(string name)
    {
        return await _context.Categories.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
    }

    public async Task<E.Category?> GetCategoryWithProductsAsync(int id)
    {
        return await _context.Categories.Include(x => x.Products).FirstOrDefaultAsync(x => x.Id == id);
    }
}
