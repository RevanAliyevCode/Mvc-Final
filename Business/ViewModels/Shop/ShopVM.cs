using System;
using Business.ViewModels.Product;
using E = Domain.Entities;

namespace Business.ViewModels.Shop;

public class ShopVM
{
    public List<ProductVM> Products { get; set; }
    public List<E.Category> Categories { get; set; }
}
