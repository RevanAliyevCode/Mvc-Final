using System;
using Domain.Entities.Base;
using Domain.Entities.Identity;

namespace Domain.Entities;

public class Comment : BaseEntity
{
    public string UserId { get; set; }
    public AppUser User { get; set; }

    public int NewsId { get; set; }
    public News News { get; set; }

    public int? ParentCommentId { get; set; }
    public Comment ParentComment { get; set; }
    public ICollection<Comment> Replies { get; set; }

    public string Content { get; set; }
}
