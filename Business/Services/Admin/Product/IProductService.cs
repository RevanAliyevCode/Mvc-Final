using System;
using Business.ViewModels.Admin.Product;

namespace Business.Services.Admin.Product;

public interface IProductService
{
    Task<ProductVM> GetAllProductAsync();
    Task<CreateProductVM> GetCreateProductVMAsync();
    Task<bool> CreateProductAsync(CreateProductVM createProductVM);
    Task<UpdateProductVM?> GetUpdateProductVMAsync(int id);
    Task<(int code, string message)> UpdateProductAsync(int id, UpdateProductVM updateProductVM);
    Task<bool> DeleteProductAsync(int id);
}
