using Product.API.Dtos.Cart.Requests;
using Product.API.Entity;

namespace Product.API.Services.Base;

public interface ICartService
{
    // public Task<bool> AddToCart(AddToCartRequest addToCartRequest);
    public Task<bool> AddToCartAsync(AddToCartRequest request);
    public Task<IEnumerable<Cart>> GetCartDetailsAsync();

    public string GetUserId(string userId);
}