using System;
using Business.Utilities.EmailService.Abstracts;
using Business.Utilities.EmailService.Concrets;
using Business.ViewModels.Account;
using Data.Repositories.Basket;
using Data.UnitOfWork;
using E = Domain.Entities;
using Domain.Entities.Identity;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;

namespace Business.Services.Account;

public class AccountService : IAccountService
{
    readonly ModelStateDictionary _modelState;
    readonly IActionContextAccessor _actionContextAccessor;
    readonly UserManager<AppUser> _userManager;
    readonly IBasketRepo _basketRepo;
    readonly IEmailSender _emailSender;
    readonly IUnitOfWork _commit;
    readonly SignInManager<AppUser> _signInManager;
    readonly RoleManager<IdentityRole> _roleManager;
    readonly LinkGenerator _linkGenerator;

    public AccountService(IActionContextAccessor actionContextAccessor, UserManager<AppUser> userManager, IBasketRepo basketRepo, IEmailSender emailSender, IUnitOfWork commit, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, LinkGenerator linkGenerator)
    {
        _modelState = actionContextAccessor.ActionContext.ModelState;
        _userManager = userManager;
        _basketRepo = basketRepo;
        _emailSender = emailSender;
        _commit = commit;
        _actionContextAccessor = actionContextAccessor;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _linkGenerator = linkGenerator;
    }

    public async Task<bool> RegisterUserAsync(SignupVM signupVM)
    {
        if (!_modelState.IsValid)
            return false;

        var user = new AppUser
        {
            UserName = signupVM.Email,
            Email = signupVM.Email,
        };

        var result = await _userManager.CreateAsync(user, signupVM.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                _modelState.AddModelError(string.Empty, error.Description);
            }
            return false;
        }


        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var confirmationLink = _linkGenerator.GetUriByAction(
            _actionContextAccessor.ActionContext.HttpContext,
            "ConfirmEmail",
            "Account",
            new { email = user.Email, token });

        _emailSender.SendEmail(new Message([user.Email], "Confirm your email", confirmationLink));

        return true;
    }

    public async Task<(int? code, string? message)> ConfirmEmailAsync(string email, string token)
    {
        if (email == null || token == null)
            return (null, null);

        var user = await _userManager.FindByEmailAsync(email);

        if (user == null) return (404, "User not found");

        var result = await _userManager.ConfirmEmailAsync(user, token);

        if (!result.Succeeded) return (400, "Invalid token");

        if (await _basketRepo.GetBasketByUserIdAsync(user.Id) != null)
            return (200, "Email confirmed successfully");

        await _basketRepo.AddAsync(new E.Basket { UserId = user.Id });
        await _commit.SaveChangesAsync();

        var role = await _roleManager.FindByNameAsync(RoleEnum.Customer.ToString());
        if (role?.Name == null)
            throw new Exception("Cannot find role: " + RoleEnum.Customer);

        var roleResult = await _userManager.AddToRoleAsync(user, role.Name);

        if (!roleResult.Succeeded)
            throw new Exception("Cannot add user to role: " + roleResult.Errors.FirstOrDefault()?.Description);

        return (200, "Email confirmed successfully");
    }

    public async Task<bool> ForgotPasswordAsync(string email)
    {
        if (email == null)
        {
            _modelState.AddModelError(string.Empty, "Email is required");
            return false;
        }

        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            _modelState.AddModelError(string.Empty, "User not found");
            return false;
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var confirmationLink = _linkGenerator.GetUriByAction(_actionContextAccessor.ActionContext.HttpContext,
        "ResetPassword",
        "Account",
        new { email = user.Email, token });

        _emailSender.SendEmail(new Message([user.Email], "ResetPassword", confirmationLink));

        return true;
    }

    public async Task<bool> LoginUserAsync(LoginVM loginVM)
    {
        if (!_modelState.IsValid)
            return false;

        var result = await _signInManager.PasswordSignInAsync(loginVM.Email, loginVM.Password, false, false);

        if (!result.Succeeded)
        {
            _modelState.AddModelError(string.Empty, "Invalid login attempt.");
            return false;
        }

        return true;
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordVM resetPasswordVM)
    {
        if (!_modelState.IsValid)
            return false;

        var user = await _userManager.FindByEmailAsync(resetPasswordVM.Email);

        if (user == null)
        {
            _modelState.AddModelError("email", "User not found.");
            return false;
        }

        var result = await _userManager.ResetPasswordAsync(user, resetPasswordVM.Token, resetPasswordVM.NewPassword);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                _modelState.AddModelError(string.Empty, error.Description);
            }
            return false;
        }

        return true;
    }

    public async Task LogoutUserAsync()
    {
        await _signInManager.SignOutAsync();
    }
}
