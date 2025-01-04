using System;
using Data.Contexts;
using Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Base;

public class BaseRepo<T> : IBaseRepository<T> where T : BaseEntity
{
    readonly DbSet<T> _dbSet;

    public BaseRepo(AppDbContext context)
    {
        _dbSet = context.Set<T>();
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }
    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }
    public void UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
    }

    public void DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
    }
}
