using System;
using System.ComponentModel.DataAnnotations;

namespace Business.ViewModels.Admin.Category;

public class CategoryCommandVM
{
    [Required(ErrorMessage = "Please enter category name")]
    public string Name { get; set; }
}
