using System;
using Business.ViewModels.Product;
using Business.ViewModels.Shop;

namespace Business.Services.Shop;

public interface IShopService
{
    Task<ShopVM> GetIndexVMAsync();
    Task<List<ProductVM>> FilterByCategory(int categoryId);
}
