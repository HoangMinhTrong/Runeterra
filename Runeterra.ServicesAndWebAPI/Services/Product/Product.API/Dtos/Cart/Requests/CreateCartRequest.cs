namespace Product.API.Dtos.Cart.Requests;

public class CreateCartRequest
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public DateTime CreateAt { get; set; }
}