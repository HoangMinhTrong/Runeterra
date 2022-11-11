using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Product.API.Entity;

namespace Product.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StoreController : ControllerBase
{
    private readonly IRequestClient<ApplicationUser> _client;

    public StoreController(IRequestClient<ApplicationUser> client)
    {
        _client = client;
    }
    
    string baseUrl = "https://localhost:5001/";
    
    
}