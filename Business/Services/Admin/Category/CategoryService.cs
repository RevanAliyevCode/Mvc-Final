using System;
using Business.ViewModels.Admin.Category;
using Data.Repositories.Category;
using Data.UnitOfWork;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using E = Domain.Entities;

namespace Business.Services.Admin.Category;

public class CategoryService : ICategoryService
{
    readonly ICategoryRepo _categoryRepository;
    readonly ModelStateDictionary _modelState;
    readonly IUnitOfWork _unitOfWork;

    public CategoryService(ICategoryRepo categoryRepository, IActionContextAccessor actionContext, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _modelState = actionContext.ActionContext.ModelState;
        _unitOfWork = unitOfWork;
    }


    public async Task<CategoryVM> GetCategoriesAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return new CategoryVM
        {
            Categories = categories.ToList()
        };
    }

    public async Task<bool> CreateCategoryAsync(CategoryCommandVM model)
    {
        if (!_modelState.IsValid)
            return false;

        if (await _categoryRepository.GetCategoryByNameAsync(model.Name) != null)
        {
            _modelState.AddModelError("Name", "Category already exists");
            return false;
        }

        var category = new E.Category
        {
            Name = model.Name
        };

        await _categoryRepository.AddAsync(category);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<CategoryCommandVM?> GetCategoryAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);

        if (category == null)
            return null;

        var model = new CategoryCommandVM
        {
            Name = category.Name
        };

        return model;
    }

    public async Task<(int? code, string? message)> UpdateCategoryAsync(int id, CategoryCommandVM model)
    {
        if (!_modelState.IsValid)
            return (null, null);

        var category = await _categoryRepository.GetByIdAsync(id);

        if (category == null)
            return (404, "Category not found");

        var existingCategory = await _categoryRepository.GetCategoryByNameAsync(model.Name);
        if (existingCategory != null && existingCategory.Id != id)
        {
            _modelState.AddModelError("Name", "Category already exists");
            return (400, "Category already exists");
        }

        category.Name = model.Name;

        _categoryRepository.UpdateAsync(category);
        await _unitOfWork.SaveChangesAsync();

        return (200, "Category updated successfully");
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);

        if (category == null)
            return false;

        _categoryRepository.DeleteAsync(category);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
