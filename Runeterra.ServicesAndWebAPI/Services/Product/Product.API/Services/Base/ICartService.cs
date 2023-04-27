using Product.API.Dtos.Cart.Requests;
using Product.API.Dtos.Cart.Responses;
using Product.API.Entity;

namespace Product.API.Services.Base;

public interface ICartService
{
    // public Task<bool> AddToCart(AddToCartRequest addToCartRequest);
    public Task<bool> AddToCartAsync(AddToCartRequest request);
    public Task<IEnumerable<ProductInCartResponse>> GetCartDetailsAsync();

    public string GetUserId(string userId);
}