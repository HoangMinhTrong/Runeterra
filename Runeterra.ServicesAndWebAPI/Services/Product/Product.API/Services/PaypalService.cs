using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PayPal.Core;
using PayPal.v1.Payments;
using Product.API.Data;
using Product.API.Dtos.Order.Requests;
using Product.API.Dtos.Paypal.Requests;
using Product.API.Entity;
using Product.API.Services.Base;
using Order = Product.API.Entity.Order;

namespace Product.API.Services;

public class PaypalService : IPaypalService
{
    private readonly ApplicationDbContext _context;
    private readonly IOptions<PaypalSettings> _paypalSettings;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICartService _cartService;
    private readonly IConfiguration _config;
    private static Order _order;
    
    public PaypalService(IOptions<PaypalSettings> paypalSettings, IHttpContextAccessor httpContextAccessor, ApplicationDbContext context, ICartService cartService, IConfiguration config)
    {
        _paypalSettings = paypalSettings;
        _httpContextAccessor = httpContextAccessor;
        _context = context;
        _cartService = cartService;
        _config = config;
    }
    
    public async Task<string> CreatePaymentAsync()
    {
        var clientId = _paypalSettings.Value.ClientId = _config.GetValue<string>("PaypalSettings:ClientId");
        var secret = _paypalSettings.Value.ClientSecret = _config.GetValue<string>("PaypalSettings:SecretKey");
        var environment = new SandboxEnvironment(clientId, secret);
        var client = new PayPalHttpClient(environment);
        var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var carts = await _context.CartDetails.Where(x => x.Cart.userId == userIdClaim).ToListAsync();
        #region Create Paypal Order

        var itemList = new ItemList()
        {
            Items = new List<Item>()
        };

        foreach (var item in carts)
        {
            itemList.Items.Add(new Item()
            {
                Name = "Test",
                Currency = "USD",
                Price = "12",
                Quantity = "1",
                Sku = "sku",
                Tax = "0"
            });
        }
        #endregion
        var paypalOrderId = DateTime.Now.Ticks;
        var hostname = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
        var payment = new Payment()
        {
            
            Intent = "sale",
            Transactions = new List<Transaction>()
            {
                new Transaction()
                {
                    Amount = new Amount()
                    {
                        Total = "12",
                        Currency = "USD"
                    },
                    ItemList = itemList,
                    Description = $"Payment for order {paypalOrderId}"
                }
            },
            RedirectUrls = new RedirectUrls()
            {
                ReturnUrl = $"{hostname}/api/paypal/capture-payment",
                CancelUrl = $"{hostname}/api/paypal/cancel-payment"
            },
            Payer = new Payer()
            {
                PaymentMethod = "paypal"
            }
        };

        var request = new PaymentCreateRequest();
        request.RequestBody(payment);

        var response = await client.Execute(request);
        var result = response.Result<Payment>();

        return result.Links.FirstOrDefault(x => x.Rel.Equals("approval_url")).Href;
    }

    public async Task<bool> CapturePaymentAsync(ConfirmCheckoutRequest confirmCheckoutRequest)
    {
        var clientId = _paypalSettings.Value.ClientId = _config.GetValue<string>("PaypalSettings:ClientId");
        var secret = _paypalSettings.Value.ClientSecret = _config.GetValue<string>("PaypalSettings:SecretKey");
        var environment = new SandboxEnvironment(clientId, secret);
        var client = new PayPalHttpClient(environment);
        var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var carts = await _context.CartDetails.Where(x => x.Cart.userId == userIdClaim).ToListAsync();
        var products = _context.Products
            .Where(p => _context.CartDetails.Any(ci => ci.productId == p.Id))
            .ToList();
        var orderDetails = new List<OrderDetail>();
        foreach (var cart in carts)
        {
            var paymentExecution = new PaymentExecution()
            {
                PayerId = userIdClaim
            };
            var request = new PaymentExecuteRequest(_order.id.ToString());
            request.RequestBody(paymentExecution);

            var response = await client.Execute(request);
            var result = response.Result<Payment>();
            
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
                id = Int32.Parse(result.Id),
                userId = userIdClaim,
                createAt = DateTime.Now,
                orderTypeId = 1001,
            };
            await _context.Orders.AddAsync(_order);
            await _context.SaveChangesAsync();

            foreach (var item in carts)
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
        }
        return true;
    }
}