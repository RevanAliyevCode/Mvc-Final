using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Business.ViewModels.Admin.Product;

public class CreateProductVM
{
    [Required(ErrorMessage = "Please enter product name")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Please enter product description")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Please enter product price")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Please select product image")]
    public IFormFile ImageFile { get; set; }

    [Required(ErrorMessage = "Please enter product stock")]
    public int Stock { get; set; }


    [Display(Name = "Category")]
    [Required(ErrorMessage = "Please select product Category")]
    public ICollection<int> CategoryIds { get; set; }

    public List<SelectListItem>? AvailableCategories { get; set; }
}
