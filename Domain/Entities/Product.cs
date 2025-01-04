using Domain.Entities.Base;

namespace Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string ImageName { get; set; }
    public int Stock { get; set; }

    public ICollection<Category> Category { get; set; }
}
