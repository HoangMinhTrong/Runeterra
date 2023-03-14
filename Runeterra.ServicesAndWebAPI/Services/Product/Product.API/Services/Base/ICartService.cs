using Product.API.Dtos.Cart.Requests;

namespace Product.API.Services.Base;

public interface ICartService
{
    public Task<bool> CreateCart(CreateCartRequest cartRequest);
    public string GetUserId(string userId);
}