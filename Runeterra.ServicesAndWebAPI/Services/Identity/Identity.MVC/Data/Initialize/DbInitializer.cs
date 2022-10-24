using System.Security.Claims;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Identity.MVC.Entity;
using Identity.MVC.Enum;
using Identity.Services.Data;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Identity.MVC.Data.Initialize;

public class DbInitializer
{
  public static void InitializeDatabase(IApplicationBuilder app)
  {
    using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
    {
      var DB_PersistedGrantDbContext = serviceScope.ServiceProvider.GetRequiredService<AppPersistedGrantDbContext>();
      DB_PersistedGrantDbContext.Database.EnsureCreated();

      try
      {
        if (DB_PersistedGrantDbContext.Database.GetPendingMigrations().Count() > 0)
        {
          DB_PersistedGrantDbContext.Database.Migrate();
        }
      }
      catch(Exception ex)
      {
                
      }
      var DB_ConfigurationDbContext = serviceScope.ServiceProvider.GetRequiredService<AppConfigurationDbContext>();
      DB_ConfigurationDbContext.Database.EnsureCreated();

      try
      {
        if (DB_ConfigurationDbContext.Database.GetPendingMigrations().Count() > 0)
        {
          DB_ConfigurationDbContext.Database.Migrate();
        }
      }
      catch(Exception ex)
      {
                
      }
      
      var DB_AppDbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
      DB_AppDbContext.Database.EnsureCreated();

      if (!DB_ConfigurationDbContext.Clients.Any())
      {
        foreach (var client in Config.Clients())
        {
          DB_ConfigurationDbContext.Clients.Add(client.ToEntity());
        }
        DB_ConfigurationDbContext.SaveChanges();
      }

      if (!DB_ConfigurationDbContext.IdentityResources.Any())
      {
        foreach (var resource in Config.IdentityResources)
        {
          DB_ConfigurationDbContext.IdentityResources.Add(resource.ToEntity());
        }
        DB_ConfigurationDbContext.SaveChanges();
      }

      if (!DB_ConfigurationDbContext.ApiScopes.Any())
      {
        foreach (var scope in Config.ApiScopes)
        {
          DB_ConfigurationDbContext.ApiScopes.Add(scope.ToEntity());
        }
        DB_ConfigurationDbContext.SaveChanges();
      }

      if (!DB_ConfigurationDbContext.ApiResources.Any())
      {
        foreach (var resource in Config.ApiResources)
        {
          DB_ConfigurationDbContext.ApiResources.Add(resource.ToEntity());
        }
        DB_ConfigurationDbContext.SaveChanges();
      }
    }

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
  public static async Task SeedSuperAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
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
      if(user==null)
      {
        await userManager.CreateAsync(defaultUser, "Password@123");
        await userManager.AddToRoleAsync(defaultUser, Enum.Roles.Admin.ToString());
      }
      var temp2 = userManager.AddClaimsAsync(defaultUser, new Claim[]
      {
        new Claim(JwtClaimTypes.Name, defaultUser.FirstName + " " + defaultUser.LastName),
        new Claim(JwtClaimTypes.GivenName, defaultUser.FirstName),
        new Claim(JwtClaimTypes.FamilyName, defaultUser.LastName),
        new Claim(JwtClaimTypes.Email, defaultUser.Email),
        new Claim(JwtClaimTypes.ClientId, defaultUser.Id),
        new Claim(JwtClaimTypes.Role, Roles.Staff),
      }).Result;
    }
  }
}
