using Microsoft.AspNetCore.Mvc;
using Product.API.Dtos.Order.Requests;
using Product.API.Services.Base;

namespace Product.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout(ConfirmCheckoutRequest confirmCheckoutRequest)
    {
        var checkout = _orderService.Checkout(confirmCheckoutRequest);
        return Ok(checkout);
    }
}