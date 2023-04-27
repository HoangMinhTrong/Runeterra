using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Product.API.Data;
using Product.API.Dtos.Paypal.Requests;
using Product.API.Services;
using Product.API.Services.Base;
using UserManagement.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers()
    .AddNewtonsoftJson(opts => opts.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

// Dependency Injection
builder.Services.AddServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthentication();
builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.Configure<PaypalSettings>((builder.Configuration.GetSection("Paypal")));
builder.Services.ConfigureSwagger();
builder.Services.AddSwaggerGen();


var app = builder.Build();
app.UseCors(builder =>
{
    builder.AllowAnyOrigin() 
        .AllowAnyMethod() // – To allow all HTTP methods.
        .AllowAnyHeader(); // – To allow all request headers.
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();