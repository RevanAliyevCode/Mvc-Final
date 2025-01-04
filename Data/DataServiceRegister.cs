using Data.Contexts;
using Data.Repositories.Basket;
using U = Data.UnitOfWork;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Data.Repositories.BasketItem;
using Data.Repositories.Category;
using Data.Repositories.Comment;
using Data.Repositories.News;
using Data.Repositories.Order;
using Data.Repositories.OrderItem;
using Data.Repositories.Product;
using Data.Repositories.NewsLetterSubscribe;

namespace Data;

public static class DataServiceRegister
{
    public static void RegisterDataServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddIdentity<AppUser, IdentityRole>(options =>
        {
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(1);
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        services.AddScoped<IBasketRepo, BasketRepo>();
        services.AddScoped<IBasketItemRepo, BasketItemRepo>();
        services.AddScoped<ICategoryRepo, CategoryRepo>();
        services.AddScoped<ICommentRepo, CommentRepo>();
        services.AddScoped<INewsRepo, NewsRepo>();
        services.AddScoped<IOrderRepo, OrderRepo>();
        services.AddScoped<IOrderItemRepo, OrderItemRepo>();
        services.AddScoped<IProductRepo, ProductRepo>();
        services.AddScoped<INewsLetterSubscribeRepo, NewsLetterSubscribeRepo>();
        services.AddScoped<U.IUnitOfWork, U.UnitOfWork>();
    }


}
