using System;
using Business.Utilities.FIleService;
using Business.ViewModels.Comment;
using Business.ViewModels.News;
using Data.Repositories.Comment;
using Data.Repositories.News;
using Data.UnitOfWork;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using E = Domain.Entities;

namespace Business.Services.News;

public class NewsService : INewsService
{
    readonly INewsRepo _newsRepo;
    readonly IUnitOfWork _unitOfWork;
    readonly ModelStateDictionary _modelState;
    readonly IFileService _fileService;
    readonly IActionContextAccessor _actionContextAccessor;
    readonly UserManager<AppUser> _userManager;
    readonly ICommentRepo _commentRepo;

    public NewsService(INewsRepo newsRepo, IUnitOfWork unitOfWork, IActionContextAccessor actionContextAccessor, IFileService fileService, UserManager<AppUser> userManager, ICommentRepo commentRepo)
    {
        _newsRepo = newsRepo;
        _unitOfWork = unitOfWork;
        _modelState = actionContextAccessor.ActionContext.ModelState;
        _fileService = fileService;
        _actionContextAccessor = actionContextAccessor;
        _userManager = userManager;
        _commentRepo = commentRepo;
    }

    public async Task<List<NewsViewModel>> GetAllNewsVMAsync()
    {
        var news = await _newsRepo.GetAllNewsWithUser();

        return news.Select(x => new NewsViewModel
        {
            Id = x.Id,
            Title = x.Title,
            Content = x.Content,
            ImageName = x.ImageName,
            UserName = x.User.UserName,
            CreatedAt = x.CreatedAt
        }).ToList();
    }

    public async Task<NewsViewModel?> GetNewsByIdVMAsync(int id)
    {
        var news = await _newsRepo.GetNewsWithUserAndCommentsById(id);

        if (news == null)
            return null;


        var commentsModel = news.Comments.Where(c => c.ParentComment == null).OrderByDescending(c => c.CreatedAt).Select(x => new CommentVM
        {
            Id = x.Id,
            Content = x.Content,
            UserName = x.User.UserName,
            CreatedAt = x.CreatedAt,
            Replies = x.Replies?.OrderByDescending(r => r.CreatedAt).Select(r => new CommentVM
            {
                Id = r.Id,
                Content = r.Content,
                UserName = r.User.UserName,
                CreatedAt = r.CreatedAt
            }).ToList() ?? new List<CommentVM>()
        }).ToList() ?? new List<CommentVM>();

        var model = new NewsViewModel
        {
            Id = news.Id,
            Title = news.Title,
            Content = news.Content,
            ImageName = news.ImageName,
            UserName = news.User.UserName,
            CreatedAt = news.CreatedAt,
            Comments = commentsModel
        };

        return model;
    }

    public async Task<bool> AddNewsAsync(CreateNewsVM model)
    {
        if (!_modelState.IsValid)
        {
            return false;
        }

        var user = await _userManager.GetUserAsync(_actionContextAccessor.ActionContext.HttpContext.User);

        if (user == null)
            return false;

        var news = new E.News
        {
            Title = model.Title,
            Content = model.Content,
            UserId = user.Id,
        };

        if (model.Image == null)
        {
            _modelState.AddModelError("Image", "Please select an image file");
            return false;
        }

        if (!_fileService.IsImage(model.Image.ContentType))
        {
            _modelState.AddModelError("Image", "Please select an image file");
            return false;
        }

        if (!_fileService.IsAvailableSize(model.Image.Length, 500))
        {
            _modelState.AddModelError("Image", "Please select an image file less than 100KB");
            return false;
        }

        news.ImageName = _fileService.Upload(model.Image, "upload/news");

        await _newsRepo.AddAsync(news);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<UpdateNewsVM?> UpdateNewsVMAsync(int id)
    {
        var news = await _newsRepo.GetByIdAsync(id);

        if (news == null)
            return null;

        var model = new UpdateNewsVM
        {
            Title = news.Title,
            Content = news.Content,
            ImageUrl = news.ImageName
        };

        return model;
    }

    public async Task<bool> UpdateNewsAsync(int id, UpdateNewsVM model)
    {
        if (!_modelState.IsValid)
            return false;

        var news = await _newsRepo.GetByIdAsync(id);

        if (news == null)
            return false;

        news.Title = model.Title;
        news.Content = model.Content;

        if (model.Image != null)
        {
            if (!_fileService.IsImage(model.Image.ContentType))
            {
                _modelState.AddModelError("Image", "Please select an image file");
                return false;
            }

            if (!_fileService.IsAvailableSize(model.Image.Length, 500))
            {
                _modelState.AddModelError("Image", "Please select an image file less than 100KB");
                return false;
            }

            news.ImageName = _fileService.Upload(model.Image, "upload/news");
        }

        _newsRepo.UpdateAsync(news);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteNewsAsync(int id)
    {
        var news = await _newsRepo.GetByIdAsync(id);

        if (news == null)
            return false;

        _newsRepo.DeleteAsync(news);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> WriteCommentAsync(int newsId, string comment)
    {
        if (string.IsNullOrWhiteSpace(comment))
            return false;

        var user = await _userManager.GetUserAsync(_actionContextAccessor.ActionContext.HttpContext.User);

        if (user == null)
            return false;

        var news = await _newsRepo.GetByIdAsync(newsId);

        if (news == null)
            return false;

        var newComment = new E.Comment
        {
            Content = comment,
            NewsId = newsId,
            UserId = user.Id
        };

        await _commentRepo.AddAsync(newComment);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> WriteReplyAsync(int newsId, int commentId, string reply)
    {
        if (string.IsNullOrWhiteSpace(reply))
            return false;

        var user = await _userManager.GetUserAsync(_actionContextAccessor.ActionContext.HttpContext.User);

        if (user == null)
            return false;

        var news = await _newsRepo.GetByIdAsync(newsId);

        if (news == null)
            return false;

        var parentComment = await _commentRepo.GetByIdAsync(commentId);

        if (parentComment == null)
            return false;

        var newReply = new E.Comment
        {
            Content = reply,
            NewsId = newsId,
            UserId = user.Id,
            ParentCommentId = commentId
        };

        await _commentRepo.AddAsync(newReply);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
