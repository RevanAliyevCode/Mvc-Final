using System;
using Data.Contexts;
using Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using E = Domain.Entities;

namespace Data.Repositories.News;

public class NewsRepo : BaseRepo<E.News>, INewsRepo
{
    readonly AppDbContext _context;
    public NewsRepo(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<E.News>> GetAllNewsWithUser()
    {
        return await _context.News.Include(x => x.User).ToListAsync();
    }

    public async Task<E.News?> GetNewsWithUserAndCommentsById(int id)
    {
        return await _context.News.Include(x => x.User).Include(x => x.Comments).ThenInclude(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
    }
}
