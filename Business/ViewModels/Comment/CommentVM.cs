using System;

namespace Business.ViewModels.Comment;

public class CommentVM
{
    public int Id { get; set; }
    public string Content { get; set; }
    public string UserName { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<CommentVM> Replies { get; set; }
}
