using System.ComponentModel.DataAnnotations;

namespace Product.API.Entity;

public class Order
{
    [Key]
    public int id { get; set; }
    public double total { get; set; }
    public DateTime createAt { get; set; }
    public string userId { get; set; }
    public int orderTypeId { get; set; }
    public ICollection<OrderDetail> OrderDetails { get; set; }
    public OrderType OrderType { get; set; }
}