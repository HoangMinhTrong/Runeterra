using System.Security.Claims;
using Product.API.Data;
using Product.API.Dtos.Cart.Requests;
using Product.API.Entity;
using Product.API.Services.Base;

namespace Product.API.Services;

public class CartService : ICartService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ApplicationDbContext _context;
    
    public CartService(IHttpContextAccessor httpContextAccessor,ApplicationDbContext context)
    {
        _httpContextAccessor = httpContextAccessor;
        _context = context;
    }
    public async Task<bool> CreateCart(CreateCartRequest cartRequest)
    {
        if (cartRequest.UserId != null)
        {
            var cart = new Cart()
            {
                id = cartRequest.Id,
                userId = GetUserId(cartRequest.UserId),
                createAt = DateTime.Now
            };
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
        }
        return true;
    }

    public string GetUserId(string userId)
    {
        var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        return userId = userIdClaim;
    }
}