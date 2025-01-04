using System;
using Business.Utilities.EmailService.Abstracts;
using Business.Utilities.EmailService.Concrets;
using Data.Repositories.NewsLetterSubscribe;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;

namespace Business.Services.Admin.SendNews;

public class SendNewsService : ISendNewsService
{
    readonly INewsLetterSubscribeRepo _newsLetterSubscribeRepo;
    readonly IEmailSender _emailSender;
    readonly LinkGenerator _linkGenerator;
    readonly IActionContextAccessor _actionContextAccessor;
    readonly ModelStateDictionary _modelState;

    public SendNewsService(INewsLetterSubscribeRepo newsLetterSubscribeRepo, IEmailSender emailSender, LinkGenerator linkGenerator, IActionContextAccessor actionContextAccessor)
    {
        _newsLetterSubscribeRepo = newsLetterSubscribeRepo;
        _emailSender = emailSender;
        _linkGenerator = linkGenerator;
        _actionContextAccessor = actionContextAccessor;
        _modelState = actionContextAccessor.ActionContext.ModelState;
    }

    public async Task<List<(string email, bool isSubscirbe)>> GetUsersVMAsync()
    {
        var subscribers = await _newsLetterSubscribeRepo.GetAllAsync();
        return subscribers.Select(s => (s.Email, s.IsSubscribed)).ToList();
    }

    public async Task<bool> SendMailAsync(string subject, string message)
    {
        if (subject == null || message == null)
        {
            _modelState.AddModelError(string.Empty, "Subject or message can not be null");
            return false;
        }

        var users = await _newsLetterSubscribeRepo.GetSubscribedUsersAsync();
        var emails = users.Select(u => u.Email).ToList();

        try
        {
            var link = _linkGenerator.GetUriByAction(_actionContextAccessor.ActionContext.HttpContext, "Index", "Home", new { area = "" });

            _emailSender.SendEmail(new Message(emails, subject, $"{message} {link}"));
        }
        catch
        {
            _modelState.AddModelError(string.Empty, "Mail sending failed");
            return false;
        }

        return true;
    }
}