using Microsoft.AspNetCore.Mvc;
using Product.API.Dtos.Cart.Requests;
using Product.API.Services.Base;

namespace Product.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpPost("cart")]
    public async Task<IActionResult> CreateCart(CreateCartRequest createCartRequest)
    {
        var cart = _cartService.CreateCart(createCartRequest);
        return Ok(cart);
    }

}