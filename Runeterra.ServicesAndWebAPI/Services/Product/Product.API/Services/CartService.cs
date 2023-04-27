using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Product.API.Data;
using Product.API.Dtos.Cart.Requests;
using Product.API.Dtos.Cart.Responses;
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
        if (cart != null && cart.ExpirationTime >= DateTime.UtcNow)
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
            .Include(x=>x.Product).ThenInclude(x=>x.Store)
            .FirstOrDefaultAsync(x => x.productId == request.ProductId && x.Product.Store.UserId == userIdClaim);

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

    public async Task<IEnumerable<ProductInCartResponse>> GetCartDetailsAsync()
    {
        var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var products = await _context.Products
            .Include(x => x.CartDetails).ThenInclude(x => x.Cart)
            .Include(x => x.Store)
            .Where(x => x.CartDetails.Any(cd =>
                cd.Cart.ExpirationTime >= DateTime.Today && cd.Cart.userId == userIdClaim)).ToListAsync();
        
        var productInCart = new List<ProductInCartResponse>();
        foreach (var product in products)
        {
            var productResponse = new ProductInCartResponse
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Description = product.Description,
                Quantity = product.CartDetails.FirstOrDefault(cd => cd.Cart.userId == userIdClaim)?.Quantity ?? 0,
            };
            productInCart.Add(productResponse);
        }
        return productInCart;
    }

    public string GetUserId(string userId)
    {
        var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        return userId = userIdClaim;
    }
}