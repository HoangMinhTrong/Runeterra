using Identity.MVC.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserManagement.API.Entity;
namespace UserManagement.API.Data.DataSeeding;

public static class SeedUser
{
    public static async Task Seeding(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        //Resident User
        var residentUser = new ApplicationUser 
        {
            UserName = "resident", 
            Email = "resident@gmail.com",
            FirstName = "Trong",
            LastName = "Hoang",
            BuildingNo = "001",
            EmailConfirmed = true, 
            PhoneNumberConfirmed = true 
        };
        if (userManager.Users.All(u => u.Id != residentUser.Id))
        {
            var user = await userManager.FindByEmailAsync(residentUser.Email);
            if(user==null)
            {
                await userManager.CreateAsync(residentUser, "Password@123");
                await userManager.AddToRoleAsync(residentUser, Roles.Resident.ToString());
            }
        }
        
        // Staff User
        var staffUser = new ApplicationUser 
        {
            UserName = "staff", 
            Email = "staff@gmail.com",
            FirstName = "Trong",
            LastName = "Hoang",
            BuildingNo = "001",
            EmailConfirmed = true, 
            PhoneNumberConfirmed = true 
        };
        if (userManager.Users.All(u => u.Id != staffUser.Id))
        {
            var user = await userManager.FindByEmailAsync(staffUser.Email);
            if(user==null)
            {
                await userManager.CreateAsync(staffUser, "Password@123");
                await userManager.AddToRoleAsync(staffUser, Roles.Staff.ToString());
            }
        }
    }
}