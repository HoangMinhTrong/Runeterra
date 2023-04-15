using Product.API.Dtos.Order.Requests;
using Product.API.Entity;

namespace Product.API.Services.Base;

public interface IPaypalService
{
    Task<string> CreatePaymentAsync();
    Task<bool> CapturePaymentAsync(string paymentId,string token, string PayerID);
}