using Identity.API.Entities;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Data.Initializer;

public static class RolesInitializer
{
    public static async Task SeedSuperAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        //Seed Default User
        var defaultUser = new ApplicationUser 
        {
            UserName = "superadmin", 
            Email = "superadmin@gmail.com",
            FirstName = "Trong",
            LastName = "Hoang",
            EmailConfirmed = true, 
            PhoneNumberConfirmed = true 
        };
        if (userManager.Users.All(u => u.Id != defaultUser.Id))
        {
            var user = await userManager.FindByEmailAsync(defaultUser.Email);
            if(user==null)
            {
                await userManager.CreateAsync(defaultUser, "Password@123");
                await userManager.AddToRoleAsync(defaultUser, Enum.Roles.Admin.ToString());
                await userManager.AddToRoleAsync(defaultUser, Enum.Roles.DirectorBoard.ToString());
                await userManager.AddToRoleAsync(defaultUser, Enum.Roles.Investor.ToString());
                await userManager.AddToRoleAsync(defaultUser, Enum.Roles.Resident.ToString());
            }
               
        }
    }
    public static async Task SeedRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        //Seed Roles
        await roleManager.CreateAsync(new IdentityRole(Enum.Roles.Admin.ToString()));
        await roleManager.CreateAsync(new IdentityRole(Enum.Roles.DirectorBoard.ToString()));
        await roleManager.CreateAsync(new IdentityRole(Enum.Roles.Investor.ToString()));
        await roleManager.CreateAsync(new IdentityRole(Enum.Roles.Resident.ToString()));
    }
}