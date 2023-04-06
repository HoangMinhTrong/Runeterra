using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Product.API.Data;
using Product.API.Dtos.Cart.Requests;
using Product.API.Entity;
using Product.API.Services.Base;

namespace Product.API.Services;

public class CartService : ICartService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ApplicationDbContext _context;
    private static Cart _cart;
    public CartService(IHttpContextAccessor httpContextAccessor,ApplicationDbContext context)
    {
        _httpContextAccessor = httpContextAccessor;
        _context = context;
    }
    public async Task<bool> AddToCartAsync(AddToCartRequest request)
    {
        var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var cart = await _context.Carts.FirstOrDefaultAsync(x => x.userId == userIdClaim);
        if (cart != null && cart.ExpirationTime >= DateTime.UtcNow && _cart.IsDelete)
        {
            _cart = cart;
        }
        else
        {
            _cart = new Cart()
            {
                userId = userIdClaim,
                createAt = DateTime.UtcNow,
                ExpirationTime = DateTime.UtcNow.AddDays(3),
                IsDelete = true
            };
            await _context.Carts.AddAsync(_cart);
            await _context.SaveChangesAsync();
        }
        
        var cartDetails = await _context.CartDetails
            .FirstOrDefaultAsync(x => x.productId == request.ProductId);

        if (cartDetails == null)
        {
            var cartDetail = new CartDetail
            {
                cartId = _cart.id,
                productId = request.ProductId,
                Quantity = request.Quantity
            };
            await _context.CartDetails.AddAsync(cartDetail);
            await _context.SaveChangesAsync();
        }
        else
        {
            cartDetails.Quantity += request.Quantity;
            _context.CartDetails.Update(cartDetails);
        }
        await _context.SaveChangesAsync();
        return true;
        
    }

    public async Task<IEnumerable<Cart>> GetCartDetailsAsync()
    {
        var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var carts = await _context.Carts
            .Include(x=>x.CartDetails)
            .ThenInclude(x=>x.Product)
            .Where(x=>x.ExpirationTime >= DateTime.Today && x.userId == userIdClaim)
            .ToListAsync();

        return carts;
    }

    public string GetUserId(string userId)
    {
        var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        return userId = userIdClaim;
    }
}