using System;

namespace Business.ViewModels.Admin.User;

public class UsersVM
{
    public string Id { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; }
}
