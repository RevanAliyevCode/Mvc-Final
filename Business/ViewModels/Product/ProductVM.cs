using System;

namespace Business.ViewModels.Product;

public class ProductVM
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string ImageName { get; set; }
    public int Stock { get; set; }
    public List<string> Categories { get; set; }
}
