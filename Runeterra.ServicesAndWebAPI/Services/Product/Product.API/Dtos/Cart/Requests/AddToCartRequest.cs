namespace Product.API.Dtos.Cart.Requests;

public class AddToCartRequest
{
    public int Quantity { get; set; }
    public int ProductId { get; set; }
   
}