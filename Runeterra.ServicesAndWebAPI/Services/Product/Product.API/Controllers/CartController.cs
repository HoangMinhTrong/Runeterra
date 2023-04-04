using Microsoft.AspNetCore.Mvc;
using Product.API.Dtos.Cart.Requests;
using Product.API.Entity;
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
    [HttpPost("items")]
    public async Task<ActionResult> AddToCartAsync(AddToCartRequest request)
    {
        var cart = await _cartService.AddToCartAsync(request);

        return Ok(cart);
    }

    [HttpGet("cart/items")]
    public async Task<ActionResult<IEnumerable<CartDetail>>> GetCartDetailsAsync()
    {
        var cartDetails = await _cartService.GetCartDetailsAsync();

        return Ok(cartDetails);
    }
   
}