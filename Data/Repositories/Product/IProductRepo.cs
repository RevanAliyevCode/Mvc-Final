using System;
using Data.Repositories.Base;
using E = Domain.Entities;

namespace Data.Repositories.Product;

public interface IProductRepo : IBaseRepository<E.Product>
{
    public Task<List<E.Product>> GetAllProductWithCategoriesAsync();
    public Task<E.Product?> GetProductWithCategoriesAsync(int id);
}
