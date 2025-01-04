using System;
using Data.Repositories.Base;
using E = Domain.Entities;

namespace Data.Repositories.Category;

public interface ICategoryRepo : IBaseRepository<E.Category>
{
    Task<E.Category?> GetCategoryByNameAsync(string name);
    Task<E.Category?> GetCategoryWithProductsAsync(int id);
}
