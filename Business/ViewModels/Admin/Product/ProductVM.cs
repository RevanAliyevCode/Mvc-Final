using System;
using E = Domain.Entities;

namespace Business.ViewModels.Admin.Product;

public class ProductVM
{
    public List<E.Product> Products { get; set; }
}
