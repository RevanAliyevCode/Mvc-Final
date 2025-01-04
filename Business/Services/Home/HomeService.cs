using System;
using Business.Services.News;
using Business.Utilities;
using Business.Utilities.EmailService.Abstracts;
using Business.Utilities.EmailService.Concrets;
using Business.ViewModels.Home;
using Business.ViewModels.Product;
using Data.Repositories.NewsLetterSubscribe;
using Data.Repositories.Product;
using Data.UnitOfWork;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using E = Domain.Entities;

namespace Business.Services.Home;

public class HomeService : IHomeService
{
    readonly IProductRepo _productRepo;
    readonly INewsLetterSubscribeRepo _newsLetterSubscribeRepo;
    readonly ModelStateDictionary _modelState;
    readonly IUnitOfWork _commit;
    readonly IActionContextAccessor _actionContextAccessor;
    readonly LinkGenerator _linkGenerator;
    readonly IEmailSender _emailSender;
    readonly INewsService _newsService;
    readonly UserManager<AppUser> _userManager;


    public HomeService(IProductRepo productRepo, IActionContextAccessor actionContextAccessor, IUnitOfWork commit, INewsLetterSubscribeRepo newsLetterSubscribe, LinkGenerator linkGenerator, IEmailSender emailSender, INewsService newsService, UserManager<AppUser> userManager)
    {
        _productRepo = productRepo;
        _modelState = actionContextAccessor.ActionContext.ModelState;
        _commit = commit;
        _actionContextAccessor = actionContextAccessor;
        _newsLetterSubscribeRepo = newsLetterSubscribe;
        _linkGenerator = linkGenerator;
        _emailSender = emailSender;
        _newsService = newsService;
        _userManager = userManager;
    }

    public async Task<HomeVM> GetIndexVMAsync()
    {
        var products = await _productRepo.GetAllProductWithCategoriesAsync();
        var news = await _newsService.GetAllNewsVMAsync();
        HomeVM model = new HomeVM
        {
            Products = products.Select(x => new ProductVM
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                ImageName = x.ImageName,
                Stock = x.Stock,
                Categories = x.Category.Select(c => c.Name).ToList(),
            }).ToList(),
            News = news
        };
        return model;
    }

    public async Task<bool> SubscribeToNewsLetterAsync(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            _modelState.AddModelError(string.Empty, "Email is required");
            return false;
        }

        if (await _newsLetterSubscribeRepo.GetNewsLetterByEmailAsync(email) != null)
        {
            _modelState.AddModelError(string.Empty, "You are already subscribed");
            return false;
        }

        var user = await _userManager.FindByEmailAsync(email);

        if (user != null)
        {

            var userSubscribed = new E.NewsLetterSubscribe
            {
                Email = email,
                IsSubscribed = true,
            };

            await _newsLetterSubscribeRepo.AddAsync(userSubscribed);
            await _commit.SaveChangesAsync();

            return true;
        }

        var token = email.Generate();
        var confirmationLink = _linkGenerator.GetUriByAction(_actionContextAccessor.ActionContext.HttpContext, "ConfirmEmail", "Home", new { email, token });

        _emailSender.SendEmail(new Message([email], "Confirm your email", confirmationLink));

        var newsLetter = new E.NewsLetterSubscribe
        {
            Email = email,
            Token = token,
        };

        await _newsLetterSubscribeRepo.AddAsync(newsLetter);
        await _commit.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ConfirmEmailAsync(string email, string token)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            return false;

        var newsLetter = await _newsLetterSubscribeRepo.GetNewsLetterByEmailAndTokenAsync(email, token);

        if (newsLetter == null)
            return false;

        if (newsLetter.IsSubscribed)
            return true;

        newsLetter.IsSubscribed = true;
        _newsLetterSubscribeRepo.UpdateAsync(newsLetter);
        await _commit.SaveChangesAsync();

        return true;
    }
}
