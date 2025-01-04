using Business;
using Data;
using C = Domain.Configs;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Stripe;
using Microsoft.AspNetCore.Mvc.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register data services
builder.Services.RegisterDataServices(builder.Configuration.GetConnectionString("DefaultConnection"));

// Register business services
builder.Services.RegisterBusinessServices();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

// Congigurations
builder.Services.Configure<C.StripeConfiguration>(builder.Configuration.GetSection("Stripe"));
builder.Services.Configure<C.EmailConfiguration>(builder.Configuration.GetSection("EmailConfiguration"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await DbInitilezer.Seed(userManager, roleManager);
}

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Value;

app.Run();
