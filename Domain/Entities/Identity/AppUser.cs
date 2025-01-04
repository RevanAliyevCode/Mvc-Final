using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Identity;

public class AppUser : IdentityUser
{
    public Basket Basket { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public ICollection<News> News { get; set; }
}
