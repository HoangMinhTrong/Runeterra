using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers;


public class AccountController : Controller
{
    // GET
    public IActionResult Index()
    {
        return Ok();
    }
}