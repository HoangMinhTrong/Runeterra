using System.ComponentModel.DataAnnotations;

namespace Product.API.Entity;

public class OrderType
{
    [Key]
    public int id { get; set; }
    public string name { get; set; }
    public OrderDetail OrderDetail { get; set; }
}