using System.Net.Http.Headers;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

using Product.API.Entity;


namespace Product.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    string baseUrl = "https://localhost:5001/";

    private readonly ILogger<UserController> _logger;
    private readonly IRequestClient<ApplicationUser> _client;

    public UserController(ILogger<UserController> logger, IRequestClient<ApplicationUser> client)
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

}