namespace Product.API.Dtos.Order.Requests;

public class CheckoutRequest
{
    public int id { get; set; }
    public int productId { get; set; }
    public int orderId { get; set; }
    public int quantity { get; set; }
    public double unitPrice { get; set; }
}