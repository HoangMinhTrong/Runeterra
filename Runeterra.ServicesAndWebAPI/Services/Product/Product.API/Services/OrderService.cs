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

        var cart = await _context.CartDetails.Where(x => x.Cart.userId == userIdClaim).Include(x => x.Product)
            .ToListAsync();
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
            var order = await _context.Orders
                .SingleOrDefaultAsync(x => x.userId == userIdClaim && x.orderTypeId == confirmCheckoutRequest.OrderTypeId && x.DeliveryId == confirmCheckout.Id);
            if(order == null) 
            { 
                order = new Order()
               {
                   userId = userIdClaim,
                   createAt = DateTime.Now,
                   orderTypeId = confirmCheckoutRequest.OrderTypeId,
                   DeliveryId = confirmCheckout.Id
               };
               await _context.Orders.AddAsync(order);
               await _context.SaveChangesAsync();
            }
           
        
            foreach (var item in cart)
            {
                var orderDetail = new OrderDetail
                {
                    productId = item.productId,
                    quantity = item.Quantity,
                    unitPrice = item.Product.Price,
                    orderId = order.id
                    
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

    public async Task<IEnumerable<Order>> GetOrders()
    {
        var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var orders = await _context.Orders
            .Include(x => x.OrderDetails)
            .ThenInclude(x => x.Product)
            .ThenInclude(x => x.Store).Where(x => x.userId == userIdClaim).ToListAsync(); 
        return orders;
    }

    public async Task<IEnumerable<Order>> GetOrdersByStore()
    {
        var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var orders = await _context.Orders
            .Include(x => x.OrderDetails)
            .ThenInclude(x => x.Product)
            .ThenInclude(x => x.Store).Where(x => x.userId == userIdClaim)
            .ToListAsync(); 
        return orders;
    }

    public async Task<IEnumerable<Entity.OrderDetail>> GetProductsByOrderId(int orderId)
    {
        var orderDetails = await _context.OrderDetails
            .Where(od => od.orderId == orderId)
            .Select(od => new { od.Product, od.quantity })
            .ToListAsync();

        return orderDetails.Select(od => new Entity.OrderDetail
        {
            Product = od.Product,
            quantity = od.quantity
        });
    }
}