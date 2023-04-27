using Microsoft.AspNetCore.Mvc;

namespace Notification.API.Controllers;

[ApiController]
[Route("[controller]")]
public class NotificationController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> SendNotification(int id)
    {
        return Ok(id);
    }
}