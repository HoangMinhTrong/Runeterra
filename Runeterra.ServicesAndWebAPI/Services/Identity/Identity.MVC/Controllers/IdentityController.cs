using Identity.MVC.Data;
using Identity.MVC.Entity;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Identity.MVC.Controllers;

public class IdentityController : ControllerBase
{
    private readonly IBus _bus;
    // private readonly IPublishEndpoint  _publishBatch;
    private readonly ApplicationDbContext _context;

    public IdentityController(IBus bus, ApplicationDbContext context)
    {
        _bus = bus;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetUser()
    {
         var userList = _context.Users.ToList();
            // Uri uri = new Uri("rabbitmq://localhost/user");
            // var endPoint = await _bus.GetSendEndpoint(uri);
            // await endPoint.SendBatch(userList);
            return Ok(userList);
    }
}