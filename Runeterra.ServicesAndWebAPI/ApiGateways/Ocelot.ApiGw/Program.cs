using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

//Config ocelot
IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("ocelot.json")
    .Build();

builder.Services.AddOcelot(configuration);
builder.Services.AddCors();

var app = builder.Build();

app.UseCors(builder =>
{
    builder.AllowAnyOrigin() 
        .AllowAnyMethod() // – To allow all HTTP methods.
        .AllowAnyHeader(); // – To allow all request headers.
});
app.UseOcelot();

app.Run();