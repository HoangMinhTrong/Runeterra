using System.Net.Http.Headers;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Product.API.Data;
using Product.API.Entity;
using Product.API.Message;

namespace Product.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
    string baseUrl = "https://localhost:5001/";
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
    
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IRequestClient<ApplicationUser> _client;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IRequestClient<ApplicationUser> client)
    {
        _logger = logger;
        _client = client;
    }
    [HttpGet(Name = "GetUser")]
    public async Task<IActionResult> AllUser()
    {
        var applicationUsers = new List<ApplicationUser>();
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var Res = await client.GetAsync("Identity/GetUser");
            if (Res.IsSuccessStatusCode)
            {
                var userResponse = Res.Content.ReadAsStringAsync().Result;
                applicationUsers = JsonConvert.DeserializeObject<List<ApplicationUser>>(userResponse);
            }
            return Ok(applicationUsers);
        }
    }

    // [HttpGet(Name = "GetWeatherForecast")]
    // public async Task<IEnumerable<WeatherForecast>> Get()
    // {
    //     return Enumerable.Range(1, 5).Select(index => new WeatherForecast
    //         {
    //             Date = DateTime.Now.AddDays(index),
    //             TemperatureC = Random.Shared.Next(-20, 55),
    //             Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    //         })
    //         .ToArray();
    // }
}