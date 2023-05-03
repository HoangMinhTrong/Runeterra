using Product.API.Dtos.Order.Requests;
using Product.API.Entity;

namespace Product.API.Services.Base;

public interface IOrderService
{
    public Task<bool> Checkout(ConfirmCheckoutRequest confirmCheckoutRequest);
    public Task<IEnumerable<Order>> GetOrders();
    public Task<IEnumerable<Order>> GetOrdersByStore();
    public Task<IEnumerable<Entity.OrderDetail>> GetProductsByOrderId(int orderId);

}