using System;
using Business.ViewModels.Account;

namespace Business.Services.Account;

public interface IAccountService
{
    Task<bool> RegisterUserAsync(SignupVM signupVM);
    Task<bool> LoginUserAsync(LoginVM loginVM);
    Task LogoutUserAsync();

    Task<(int? code, string? message)> ConfirmEmailAsync(string email, string token);

    Task<bool> ForgotPasswordAsync(string email);

    Task<bool> ResetPasswordAsync(ResetPasswordVM resetPasswordVM);
}
