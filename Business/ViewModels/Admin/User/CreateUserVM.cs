using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Business.ViewModels.Admin.User;

public class CreateUserVM
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public List<string> RoleIds { get; set; }
    public List<SelectListItem>? Roles { get; set; }
}
