using System;
using Data.Contexts;

namespace Data.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}
