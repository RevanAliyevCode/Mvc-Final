using Domain.Entities.Identity;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Data;

public static class DbInitilezer
{
    public async static Task Seed(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        await SeedRoles(roleManager);
        await SeedAdmin(userManager, roleManager);
    }

    private async static Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        foreach (var role in Enum.GetNames<RoleEnum>())
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var identityRole = new IdentityRole { Name = role };
                var result = await roleManager.CreateAsync(identityRole);
                if (!result.Succeeded)
                {
                    throw new Exception("Cannot create role: " + result.Errors.FirstOrDefault()?.Description);
                }
            }
        }
    }

    private async static Task SeedAdmin(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        if (await userManager.FindByEmailAsync("admin@gmail.com") == null)
        {
            var admin = new AppUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
            };

            var result = await userManager.CreateAsync(admin, "Admin123-");
            if (!result.Succeeded)
                throw new Exception("Cannot create admin: " + result.Errors.FirstOrDefault()?.Description);

            var role = await roleManager.FindByNameAsync(RoleEnum.Admin.ToString());
            if (role?.Name == null)
                throw new Exception("Cannot find role: " + RoleEnum.Admin);

            result = await userManager.AddToRoleAsync(admin, role.Name);

            if (!result.Succeeded)
                throw new Exception("Cannot add user to role: " + result.Errors.FirstOrDefault()?.Description);
        }
    }
}
