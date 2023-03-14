using System.ComponentModel.DataAnnotations;

namespace Product.API.Entity;

public class CartDetail
{
    [Key]
    public int id { get; set; }
    public int productId { get; set; }
    public int cartId { get; set; }
    public int  Quantity { get; set; }
    public Cart Cart { get; set; }
    public Product Product { get; set; }
}