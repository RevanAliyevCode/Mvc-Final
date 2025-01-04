using System;
using Business.Utilities.FIleService;
using Business.ViewModels.Admin.Product;
using Data.Repositories.Category;
using Data.Repositories.Product;
using Data.UnitOfWork;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using E = Domain.Entities;

namespace Business.Services.Admin.Product;

public class ProductService : IProductService
{
    readonly IFileService _fileService;
    readonly IProductRepo _productRepo;
    readonly ICategoryRepo _categoryRepo;
    readonly IUnitOfWork _unitOfWork;
    readonly ModelStateDictionary _modelState;

    public ProductService(IProductRepo productRepo, ICategoryRepo categoryRepo, IUnitOfWork unitOfWork, IFileService fileService, IActionContextAccessor actionContextAccessor)
    {
        _productRepo = productRepo;
        _unitOfWork = unitOfWork;
        _fileService = fileService;
        _categoryRepo = categoryRepo;
        _modelState = actionContextAccessor.ActionContext.ModelState;
    }

    public async Task<ProductVM> GetAllProductAsync()
    {
        var products = await _productRepo.GetAllProductWithCategoriesAsync();

        ProductVM model = new ProductVM
        {
            Products = products
        };

        return model;
    }


    public async Task<CreateProductVM> GetCreateProductVMAsync()
    {
        var categories = await _categoryRepo.GetAllAsync();
        CreateProductVM model = new CreateProductVM()
        {
            AvailableCategories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList()
        };

        return model;
    }

    public async Task<bool> CreateProductAsync(CreateProductVM createProductVM)
    {
        if (!_modelState.IsValid)
            return false;

        var product = new E.Product
        {
            Name = createProductVM.Name,
            Description = createProductVM.Description,
            Price = createProductVM.Price,
            Stock = createProductVM.Stock,
            Category = new List<E.Category>()
        };

        if (createProductVM.ImageFile == null)
        {
            _modelState.AddModelError("ImageFile", "Please select an image file");
            return false;
        }

        if (!_fileService.IsImage(createProductVM.ImageFile.ContentType))
        {
            _modelState.AddModelError("ImageFile", "Please select an image file");
            return false;
        }

        if (!_fileService.IsAvailableSize(createProductVM.ImageFile.Length, 500))
        {
            _modelState.AddModelError("ImageFile", "Please select an image file less than 100KB");
            return false;
        }

        product.ImageName = _fileService.Upload(createProductVM.ImageFile, "upload/product");


        List<E.Category> categories = await _categoryRepo.GetAllAsync();
        categories = categories.Where(c => createProductVM.CategoryIds.Contains(c.Id)).ToList();

        product.Category = categories;

        await _productRepo.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<UpdateProductVM?> GetUpdateProductVMAsync(int id)
    {
        var product = await _productRepo.GetProductWithCategoriesAsync(id);

        if (product == null)
            return null;

        UpdateProductVM model = new UpdateProductVM
        {
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            ImageName = product.ImageName,
            Stock = product.Stock,
            CategoryIds = product.Category.Select(c => c.Id).ToList(),
            AvailableCategories = (await _categoryRepo.GetAllAsync()).Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList()
        };

        return model;
    }

    public async Task<(int code, string message)> UpdateProductAsync(int id, UpdateProductVM updateProductVM)
    {
        if (!_modelState.IsValid)
            return (400, "Invalid data");

        var product = await _productRepo.GetProductWithCategoriesAsync(id);

        if (product == null)
            return (404, "Product not found");

        product.Name = updateProductVM.Name;
        product.Description = updateProductVM.Description;
        product.Price = updateProductVM.Price;
        product.Stock = updateProductVM.Stock;

        if (updateProductVM.ImageFile != null)
        {
            if (!_fileService.IsImage(updateProductVM.ImageFile.ContentType))
            {
                _modelState.AddModelError("ImageFile", "Please select an image file");
                return (400, "Please select an image file");
            }

            if (!_fileService.IsAvailableSize(updateProductVM.ImageFile.Length, 500))
            {
                _modelState.AddModelError("ImageFile", "Please select an image file less than 100KB");
                return (400, "Please select an image file less than 100KB");
            }

            _fileService.Delete(product.ImageName, "upload/product");
            product.ImageName = _fileService.Upload(updateProductVM.ImageFile, "upload/product");
        }

        List<E.Category> categories = await _categoryRepo.GetAllAsync();
        categories = categories.Where(c => updateProductVM.CategoryIds.Contains(c.Id)).ToList();

        product.Category = categories;

        _productRepo.UpdateAsync(product);
        await _unitOfWork.SaveChangesAsync();

        return (200, "Product updated successfully");
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _productRepo.GetByIdAsync(id);

        if (product == null)
            return false;

        _fileService.Delete(product.ImageName, "upload/product");

        _productRepo.DeleteAsync(product);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

}
