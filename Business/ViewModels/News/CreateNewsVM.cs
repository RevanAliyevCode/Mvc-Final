using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Business.ViewModels.News;

public class CreateNewsVM
{
    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Content is required")]
    public string Content { get; set; }

    [Required(ErrorMessage = "Image is required")]
    public IFormFile Image { get; set; }
}
