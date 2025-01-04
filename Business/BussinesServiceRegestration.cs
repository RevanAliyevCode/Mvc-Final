using System;
using Business.Services.Account;
using Business.Services.Admin.Category;
using Business.Services.Admin.Product;
using Business.Services.Admin.SendNews;
using Business.Services.Admin.User;
using Business.Services.Basket;
using Business.Services.Cart;
using Business.Services.Home;
using Business.Services.News;
using Business.Services.Payment;
using Business.Services.Shop;
using Business.Utilities.EmailService.Abstracts;
using Business.Utilities.EmailService.Concrets;
using Business.Utilities.FIleService;
using Microsoft.Extensions.DependencyInjection;

namespace Business;

public static class BussinesServiceRegestration
{
    public static void RegisterBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IEmailSender, EmailSender>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IHomeService, HomeService>();
        services.AddScoped<IShopService, ShopService>();
        services.AddScoped<ISendNewsService, SendNewsService>();
        services.AddScoped<IBasketService, BasketService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<INewsService, NewsService>();
    }
}
