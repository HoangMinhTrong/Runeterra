var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddMvc();
builder.Services.AddControllersWithViews();
//IndentityServer4
builder.Services.AddIdentityServer();

var app = builder.Build();


app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();