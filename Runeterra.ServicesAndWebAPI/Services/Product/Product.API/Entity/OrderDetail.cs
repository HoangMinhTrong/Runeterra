using System.ComponentModel.DataAnnotations;

namespace Product.API.Entity;

public class OrderDetail
{
    [Key]
    public int id { get; set; }
    public int productId { get; set; }
    public int orderId { get; set; }
    public int quantity { get; set; }
    public double unitPrice { get; set; }
    public Order Order { get; set; }
    public Product Product { get; set; }
}