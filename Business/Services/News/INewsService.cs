using System;
using Business.ViewModels.News;
using E = Domain.Entities;

namespace Business.Services.News;

public interface INewsService
{
    Task<List<NewsViewModel>> GetAllNewsVMAsync();
    Task<NewsViewModel?> GetNewsByIdVMAsync(int id);
    Task<bool> AddNewsAsync(CreateNewsVM model);
    Task<UpdateNewsVM?> UpdateNewsVMAsync(int id);
    Task<bool> UpdateNewsAsync(int id, UpdateNewsVM model);
    Task<bool> DeleteNewsAsync(int id);
    Task<bool> WriteCommentAsync(int newsId, string comment);
    Task<bool> WriteReplyAsync(int newsId, int commentId, string reply);
}
