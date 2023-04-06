using System.ComponentModel.DataAnnotations;

namespace Product.API.Entity;

public class DeliveryAddress
{
    [Key]
    public int Id { get; set; }
    public string Address { get; set; }
    public string BuildingNo { get; set; }
    public string ApartmentNo { get; set; }
    public IEnumerable<Order> Orders { get; set; }
}