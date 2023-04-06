using Product.API.Dtos.Order.Requests;
using Product.API.Entity;

namespace Product.API.Services.Base;

public interface IOrderService
{
    public Task<bool> Checkout(ConfirmCheckoutRequest confirmCheckoutRequest);
  
}