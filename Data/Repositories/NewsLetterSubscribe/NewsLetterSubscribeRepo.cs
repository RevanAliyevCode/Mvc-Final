using System;
using Data.Contexts;
using Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using E = Domain.Entities;

namespace Data.Repositories.NewsLetterSubscribe;

public class NewsLetterSubscribeRepo : BaseRepo<E.NewsLetterSubscribe>, INewsLetterSubscribeRepo
{
    readonly AppDbContext _context;
    public NewsLetterSubscribeRepo(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<E.NewsLetterSubscribe?> GetNewsLetterByEmailAndTokenAsync(string email, string token)
    {
        return await _context.NewsLetterSubscribes.FirstOrDefaultAsync(x => x.Email == email && x.Token == token);
    }

    public async Task<E.NewsLetterSubscribe?> GetNewsLetterByEmailAsync(string email)
    {
        return await _context.NewsLetterSubscribes.FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<List<E.NewsLetterSubscribe>> GetSubscribedUsersAsync()
    {
        return await _context.NewsLetterSubscribes.Where(x => x.IsSubscribed).ToListAsync();
    }
}
