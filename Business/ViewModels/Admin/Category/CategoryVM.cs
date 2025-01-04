using System;
using E = Domain.Entities;

namespace Business.ViewModels.Admin.Category;

public class CategoryVM
{
    public List<E.Category> Categories { get; set; }
}
