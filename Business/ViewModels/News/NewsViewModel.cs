using System;
using Business.ViewModels.Comment;
using Domain.Entities;

namespace Business.ViewModels.News;

public class NewsViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string ImageName { get; set; }
    public string UserName { get; set; }

    public DateTime CreatedAt { get; set; }
    public List<CommentVM> Comments { get; set; }
}
