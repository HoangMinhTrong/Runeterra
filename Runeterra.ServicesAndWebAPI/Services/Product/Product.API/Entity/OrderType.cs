using System.ComponentModel.DataAnnotations;

namespace Product.API.Entity;

public class OrderType
{
    [Key]
    public int id { get; set; }
    public string name { get; set; }
    public IEnumerable<Order> Orders { get; set; }
}