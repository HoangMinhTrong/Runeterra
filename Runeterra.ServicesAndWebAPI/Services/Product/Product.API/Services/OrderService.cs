using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Product.API.Data;
using Product.API.Dtos.Order.Requests;
using Product.API.Entity;
using Product.API.Services.Base;

namespace Product.API.Services;

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;
    private readonly ICartService _cartService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private static Order _order;

    public OrderService(ICartService cartService, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _cartService = cartService;
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<bool> Checkout(ConfirmCheckoutRequest confirmCheckoutRequest)
    {
        var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
       
            var cart = await _context.CartDetails.Where(x => x.Cart.userId == userIdClaim).ToListAsync();
            var products = _context.Products
                .Where(p => _context.CartDetails.Any(ci => ci.productId == p.Id))
                .ToList();
            var orderDetails = new List<OrderDetail>();
            var confirmCheckout = new DeliveryAddress()
            {
                Address = confirmCheckoutRequest.Address,
                ApartmentNo = confirmCheckoutRequest.ApartmentNo,
                BuildingNo = confirmCheckoutRequest.BuildingNo,
            };
            await _context.AddAsync(confirmCheckout);
            await _context.SaveChangesAsync();
            //
           
            _order = new Order()
                {
                    userId = userIdClaim,
                    createAt = DateTime.Now,
                    orderTypeId = confirmCheckoutRequest.OrderTypeId,
                    DeliveryId = confirmCheckout.Id
                };
                await _context.Orders.AddAsync(_order);
                await _context.SaveChangesAsync();
        
            foreach (var item in cart)
            {
                var product = products.FirstOrDefault(p => p.Id == item.productId);
                var orderDetail = new OrderDetail
                {
                    productId = item.productId,
                    quantity = item.Quantity,
                    unitPrice = product.Price,
                    orderId = _order.id
                    
                };
                orderDetails.Add(orderDetail);
            }
            await _context.OrderDetails.AddRangeAsync(orderDetails);
            await _context.SaveChangesAsync();

            var clearCart = await _cartService.GetCartDetailsAsync();
            _context.RemoveRange(clearCart);
            await _context.SaveChangesAsync();
            return true;
    }
    
}