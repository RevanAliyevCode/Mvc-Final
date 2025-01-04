using System;
using Business.ViewModels.Admin.User;
using Domain.Entities.Identity;

namespace Business.Services.Admin.User;

public interface IUserService
{
    Task<List<UsersVM>> GetAllUsersAsync();
    Task<AppUser?> GetUserAsync(string userId);
    Task<CreateUserVM> GetCreateUserVMAsync();
    Task<bool> CreateUserAsync(CreateUserVM createUserVM);
    Task<UpdateUserVM?> GetUpdateUserVMAsync(string userId);
    Task<bool> UpdateUserAsync(string userId, UpdateUserVM updateUserVM);
    Task<bool> DeleteUserAsync(string userId);
}
