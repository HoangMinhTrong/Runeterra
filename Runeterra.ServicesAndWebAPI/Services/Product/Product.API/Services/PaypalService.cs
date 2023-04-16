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
        var carts = await _context.CartDetails.Where(x => x.Cart.userId == userIdClaim).Include(x => x.Product)
            .ToListAsync();
        List<double> totalList = new List<double>();
        #region Create Paypal Order

        var itemList = new ItemList()
        {
            Items = new List<Item>()
        };  
        
        foreach (var item in carts)
        {
            itemList.Items.Add(new Item()
            {
                Name = item.Product.Name,
                Description = item.productId.ToString(),
                Currency = "USD",
                Price = item.Product.Price.ToString(),
                Quantity = item.Quantity.ToString(),
                Sku = userIdClaim,
                Tax = "0"
            });
            var total = item.Quantity * item.Product.Price;
            totalList.Add(total);
        }
        var grandTotal = totalList.Sum();
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
                        
                        Total = grandTotal.ToString(),
                        Currency = "USD"
                    },
                    ItemList = itemList,
                    Description = $"Payment for order {paypalOrderId}"
                }
            },
            RedirectUrls = new RedirectUrls()
            {
                ReturnUrl = $"{hostname}/api/order/capture-payment",
                CancelUrl = $"{hostname}/api/order/cancel-payment"
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

    public async Task<bool> CapturePaymentAsync(string paymentId,string token, string PayerID)
    {
        var clientId = _paypalSettings.Value.ClientId = _config.GetValue<string>("PaypalSettings:ClientId");
        var secret = _paypalSettings.Value.ClientSecret = _config.GetValue<string>("PaypalSettings:SecretKey");
        var environment = new SandboxEnvironment(clientId, secret);
        var client = new PayPalHttpClient(environment);
        var products = _context.Products
            .Where(p => _context.CartDetails.Any(ci => ci.productId == p.Id))
            .ToList();

        
        var paymentExecution = new PaymentExecution()
            {
                PayerId = PayerID
            };
            var request = new PaymentExecuteRequest(paymentId);
            request.RequestBody(paymentExecution);

            var response = await client.Execute(request);
            var result = response.Result<Payment>();
           
            var confirmCheckout = new DeliveryAddress()
            {
                Address = result.Payer.PayerInfo.ShippingAddress.Line1,
                ApartmentNo = "test",
                BuildingNo = result.Payer.PayerInfo.ShippingAddress.State,
            };
            await _context.AddAsync(confirmCheckout);
            await _context.SaveChangesAsync();
            
            var orderDetails = new List<OrderDetail>();
            foreach (var item in result.Transactions)
            {
                foreach (var skuItem in item.ItemList.Items)
                {
                    _order = new Order()
                    {
                        userId = skuItem.Sku,
                        createAt = DateTime.Now,
                        orderTypeId = 1001,
                        DeliveryId = confirmCheckout.Id,
                        total = double.Parse(item.Amount.Total) 
                    };
                    await _context.Orders.AddAsync(_order);
                    await _context.SaveChangesAsync();
                    
                    var product = products.FirstOrDefault(x => x.Id.ToString() == skuItem.Description);
                    if (product == null)
                    {
                        // handle null case
                    }

                    var orderDetail = new OrderDetail
                    {
                        productId = product.Id,
                        quantity = int.Parse(skuItem.Quantity),
                        unitPrice = product.Price,
                        orderId = _order.id
                    };
                    orderDetails.Add(orderDetail);
                }
            }

            await _context.OrderDetails.AddRangeAsync(orderDetails);
            await _context.SaveChangesAsync();

            var clearCart = await _context.Carts.ToListAsync();
            _context.RemoveRange(clearCart);
            await _context.SaveChangesAsync();
            return true;
    }
}