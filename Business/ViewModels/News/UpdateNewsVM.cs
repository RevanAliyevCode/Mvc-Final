using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Business.ViewModels.News;

public class UpdateNewsVM
{
    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Content is required")]
    public string Content { get; set; }

    public IFormFile? Image { get; set; }
    public string? ImageUrl { get; set; }

}
