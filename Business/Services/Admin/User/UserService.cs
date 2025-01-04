using System;
using Business.ViewModels.Admin.User;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Business.Services.Admin.User;

public class UserService : IUserService
{
    readonly UserManager<AppUser> _userManager;
    readonly RoleManager<IdentityRole> _roleManager;
    readonly ModelStateDictionary _modelState;

    public UserService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IActionContextAccessor actionContextAccessor)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _modelState = actionContextAccessor.ActionContext.ModelState;
    }

    public async Task<List<UsersVM>> GetAllUsersAsync()
    {
        var model = new List<UsersVM>();
        foreach (var user in _userManager.Users.ToList())
        {
            if (!_userManager.IsInRoleAsync(user, "Admin").Result)
            {
                var userRoles = _userManager.GetRolesAsync(user).Result;
                model.Add(new UsersVM
                {
                    Id = user.Id,
                    Email = user.Email,
                    Roles = userRoles.ToList()
                });
            }
        }

        return model;
    }

    public async Task<AppUser?> GetUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return user;
    }

    public async Task<CreateUserVM> GetCreateUserVMAsync()
    {
        var model = new CreateUserVM();
        var roles = await _roleManager.Roles.ToListAsync();
        model.Roles = roles.Where(r => r.Name != "Admin").Select(r => new SelectListItem
        {
            Text = r.Name,
            Value = r.Id
        }).ToList();

        return model;
    }

    public async Task<bool> CreateUserAsync(CreateUserVM createUserVM)
    {
        if (!_modelState.IsValid)
            return false;

        var user = new AppUser
        {
            Email = createUserVM.Email,
            UserName = createUserVM.Email
        };
        var result = await _userManager.CreateAsync(user, createUserVM.Password);
        if (result.Succeeded)
        {
            foreach (var roleId in createUserVM.RoleIds)
            {
                var role = await _roleManager.FindByIdAsync(roleId);
                if (role?.Name != null)
                {
                    _userManager.AddToRoleAsync(user, role.Name).Wait();
                }
            }
            return true;
        }

        foreach (var error in result.Errors)
        {
            _modelState.AddModelError("", error.Description);
        }
        return false;
    }

    public async Task<UpdateUserVM?> GetUpdateUserVMAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            var model = new UpdateUserVM
            {
                Email = user.Email
            };
            var roles = await _roleManager.Roles.ToListAsync();

            model.Roles = roles.Where(r => r.Name != "Admin").Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id
            }).ToList();

            var userRoleNames = await _userManager.GetRolesAsync(user);
            model.RoleIds = roles
                .Where(r => userRoleNames.Contains(r.Name))
                .Select(r => r.Id)
                .ToList();

            return model;
        }

        return null;
    }

    public async Task<bool> UpdateUserAsync(string userId, UpdateUserVM updateUserVM)
    {
        if (!_modelState.IsValid)
            return false;

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            _modelState.AddModelError("", "User not found");
            return false;
        }

        if (updateUserVM.Email != null && updateUserVM.Email != user.Email)
        {
            user.Email = updateUserVM.Email;
            user.UserName = updateUserVM.Email;
        }

        if (updateUserVM.NewPassword != null)
        {
            var newPassword = _userManager.PasswordHasher.HashPassword(user, updateUserVM.NewPassword);
            user.PasswordHash = newPassword;
        }

        var userRoles = new List<string>();

        foreach (var role in _roleManager.Roles.ToList())
        {
            if (_userManager.IsInRoleAsync(user, role.Name).Result)
            {
                userRoles.Add(role.Id);
            }
        }

        var selectedRoles = updateUserVM.RoleIds.Except(userRoles).ToList();
        var unselectedRoles = userRoles.Except(updateUserVM.RoleIds).ToList();

        foreach (var roleId in selectedRoles)
        {
            var role = _roleManager.FindByIdAsync(roleId).Result;
            if (role != null)
            {
                _userManager.AddToRoleAsync(user, role.Name).Wait();
            }
        }

        foreach (var roleId in unselectedRoles)
        {
            var role = _roleManager.FindByIdAsync(roleId).Result;
            if (role != null)
            {
                _userManager.RemoveFromRoleAsync(user, role.Name).Wait();
            }
        }

        var result = _userManager.UpdateAsync(user).Result;
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                _modelState.AddModelError("", error.Description);
            }
            return false;
        }

        return true;
    }

    public async Task<bool> DeleteUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user != null)
        {
            var result = _userManager.DeleteAsync(user).Result;
            if (result.Succeeded)
            {
                return true;
            }
            foreach (var error in result.Errors)
            {
                _modelState.AddModelError("", error.Description);
            }
        }
        return false;
    }

}
