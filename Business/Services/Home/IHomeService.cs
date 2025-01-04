using System;
using Business.ViewModels.Home;
using Business.ViewModels.Product;

namespace Business.Services.Home;

public interface IHomeService
{
    Task<HomeVM> GetIndexVMAsync();
    Task<bool> SubscribeToNewsLetterAsync(string email);
    Task<bool> ConfirmEmailAsync(string email, string token);
}
