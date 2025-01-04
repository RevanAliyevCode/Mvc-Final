using System;
using Domain.Entities.Base;
using Domain.Entities.Identity;

namespace Domain.Entities;

public class News : BaseEntity
{
    public string UserId { get; set; }
    public AppUser User { get; set; }

    public string Title { get; set; }
    public string Content { get; set; }
    public string ImageName { get; set; }

    public ICollection<Comment> Comments { get; set; }
}
