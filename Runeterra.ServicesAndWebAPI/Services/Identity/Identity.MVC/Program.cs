using System.Configuration;
using Duende.IdentityServer.EntityFramework.Options;
using Identity.MVC;
using IdentityServerHost.Quickstart.UI;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

var connectionString = configuration.GetConnectionString("DefaultConnection");

var migrationsAssembly = typeof(Config).Assembly.GetName().Name;

// Add services to the container.
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
    .AddTestUsers(TestUsers.Users);
// connection string
    builderDuende.AddConfigurationStore(options => 
        options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
            opt => opt.MigrationsAssembly(migrationsAssembly)));
    builderDuende.AddOperationalStore(options =>
        options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
            opt => opt.MigrationsAssembly(migrationsAssembly)));
    //


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

app.Run();