namespace Product.API.Dtos.Paypal.Requests;

public class PaypalSettings
{
    public string ClientId  { get; set; }
    public string ClientSecret { get; set; }
    public string Environment { get; set; }
}