using System.Configuration;
using Duende.IdentityServer.EntityFramework.Options;
using Identity.MVC;
using Identity.MVC.Data;
using Identity.MVC.Data.Initialize;
using Identity.MVC.Entity;
using Identity.Services.Data;
using IdentityServerHost.Quickstart.UI;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();
//
var builderDuende = builder.Services
    .AddIdentityServer(options =>
    {
        // set path where to store keys
        options.KeyManagement.KeyPath = "/Users/Shared/Keys";
    
        // new key every 30 days
        options.KeyManagement.RotationInterval = TimeSpan.FromDays(30);
    
        // announce new key 2 days in advance in discovery
        options.KeyManagement.PropagationTime = TimeSpan.FromDays(2);
    
        // keep old key for 7 days in discovery for validation of tokens
        options.KeyManagement.RetentionDuration = TimeSpan.FromDays(7);
        options.Events.RaiseErrorEvents = true;
        options.Events.RaiseInformationEvents = true;
        options.Events.RaiseFailureEvents = true;
        options.Events.RaiseSuccessEvents = true;


        options.EmitStaticAudienceClaim = true;
    })
    .AddAspNetIdentity<ApplicationUser>();


    // codes, tokens, consents
    builderDuende.AddOperationalStore<AppPersistedGrantDbContext>(options =>
        options.ConfigureDbContext = option =>
            option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    // clients, resources
    builderDuende.AddConfigurationStore<AppConfigurationDbContext>(options =>
        options.ConfigureDbContext = option =>
            option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


    builder.Services.AddAuthentication();
// in-memory, code config
    



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


// Seed 
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    try
    {
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await DbInitializer.SeedSuperAdminAsync(userManager, roleManager);
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}
DbInitializer.InitializeDatabase(app);

app.Run();