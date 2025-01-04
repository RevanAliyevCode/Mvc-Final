using System;
using Business.ViewModels.Admin.Category;

namespace Business.Services.Admin.Category;

public interface ICategoryService
{
    Task<CategoryVM> GetCategoriesAsync();
    Task<bool> CreateCategoryAsync(CategoryCommandVM model);
    Task<CategoryCommandVM?> GetCategoryAsync(int id);
    Task<(int? code, string? message)> UpdateCategoryAsync(int id, CategoryCommandVM model);
    Task<bool> DeleteCategoryAsync(int id);
}
