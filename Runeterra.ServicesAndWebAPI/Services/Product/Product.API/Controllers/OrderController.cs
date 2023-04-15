using Microsoft.AspNetCore.Mvc;
using PayPal.v1.Payments;
using Product.API.Dtos.Order.Requests;
using Product.API.Entity;
using Product.API.Services.Base;
using Payer = PayPal.v1.Orders.Payer;

namespace Product.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IPaypalService _paypalService;
    public OrderController(IOrderService orderService, IPaypalService paypalService)
    {
        _orderService = orderService;
        _paypalService = paypalService;
    }

    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout(ConfirmCheckoutRequest confirmCheckoutRequest)
    {
        var checkout = _orderService.Checkout(confirmCheckoutRequest);
        return Ok(checkout);
    }
    [HttpPost("create-payment")]
    public async Task<IActionResult> CreatePaymentAsync()
    {
        var approvalUrl = await _paypalService.CreatePaymentAsync();
        return Ok(new { approvalUrl});
    }

    [HttpGet("capture-payment")]
    public async Task<IActionResult> CapturePaymentAsync(string paymentId,string token, string PayerID)
    {
        var order = await _paypalService.CapturePaymentAsync(paymentId, token,PayerID);
        return Ok(order);
    }

    [HttpGet("cancel-payment")]
    public IActionResult CancelPaymentAsync()
    {
        return Ok("Payment cancelled.");
    }
}