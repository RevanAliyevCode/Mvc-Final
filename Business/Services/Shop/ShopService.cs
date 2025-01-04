using System;
using Business.ViewModels.Product;
using Business.ViewModels.Shop;
using Data.Repositories.Category;
using Data.Repositories.Product;

namespace Business.Services.Shop;

public class ShopService : IShopService
{
    readonly IProductRepo _productRepo;
    readonly ICategoryRepo _categoryRepo;

    public ShopService(IProductRepo productRepo, ICategoryRepo categoryRepo)
    {
        _productRepo = productRepo;
        _categoryRepo = categoryRepo;
    }

    public async Task<ShopVM> GetIndexVMAsync()
    {
        var products = await _productRepo.GetAllProductWithCategoriesAsync();
        ShopVM model = new ShopVM
        {
            Products = products.Select(x => new ProductVM
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                ImageName = x.ImageName,
                Stock = x.Stock,
                Categories = x.Category.Select(c => c.Name).ToList(),
            }).ToList(),
            Categories = await _categoryRepo.GetAllAsync(),
        };

        return model;
    }

    public async Task<List<ProductVM>> FilterByCategory(int categoryId)
    {
        if (categoryId == 0)
        {
            var products = await _productRepo.GetAllAsync();
            return products.Select(x => new ProductVM
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                ImageName = x.ImageName,
            }).ToList();
        }

        var category = await _categoryRepo.GetCategoryWithProductsAsync(categoryId);

        if (category == null)
        {
            return [];
        }

        var model = category.Products.Select(x => new ProductVM
        {
            Id = x.Id,
            Name = x.Name,
            Price = x.Price,
            ImageName = x.ImageName,
        }).ToList();

        return model;
    }
}

