using System.Security.Claims;

using Identity.MVC.Enum;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using UserManagement.API.Data;
using UserManagement.API.Entity;

namespace Identity.MVC.Data.Initialize;

public class DbInitializer
{
  public static void InitializeDatabase(IApplicationBuilder app)
  {


    using (var serviceScope = app.ApplicationServices.CreateScope())
    {
      var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
      context.Database.EnsureCreated();

      var _userManager =
        serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
      var _roleManager =
        serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
      try
      {
        if (context.Database.GetPendingMigrations().Any())
        {
          context.Database.Migrate();
        }
      }
      catch (Exception ex)
      {

      }

      if (_roleManager.FindByNameAsync(Roles.Admin).Result == null)
      {
        var adminRole = new IdentityRole(Roles.Admin);
        _roleManager.CreateAsync(adminRole).GetAwaiter().GetResult();
        var staffRole = new IdentityRole(Roles.Staff);
        _roleManager.CreateAsync(staffRole).GetAwaiter().GetResult();
        var studentRole = new IdentityRole(Roles.Resident);
        _roleManager.CreateAsync(studentRole).GetAwaiter().GetResult();
      }
      else
      {
        return;
      }
    }
  }

  public static async Task SeedSuperAdminAsync(UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager)
  {
    //Seed Default User
    var defaultUser = new ApplicationUser
    {
      UserName = "superadmin",
      Email = "superadmin@gmail.com",
      FirstName = "Trong",
      LastName = "Hoang",
      BuildingNo = "001",
      EmailConfirmed = true,
      PhoneNumberConfirmed = true
    };
    if (userManager.Users.All(u => u.Id != defaultUser.Id))
    {
      var user = await userManager.FindByEmailAsync(defaultUser.Email);
      if (user == null)
      {
        await userManager.CreateAsync(defaultUser, "Password@123");
        await userManager.AddToRoleAsync(defaultUser, Enum.Roles.Admin.ToString());
      }
    }
  }
}