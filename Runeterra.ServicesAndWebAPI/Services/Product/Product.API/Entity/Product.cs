using System.ComponentModel.DataAnnotations;

namespace Product.API.Entity;

public class Product
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public bool Status { get; set; }
    public int Quantity { get; set; }
    public string ImageUrl { get; set; }
    public int ProductTypeId { get; set; }
    public int StoreId { get; set; }
    public Store Store { get; set; }
    public ProductType ProductType { get; set; }
    public ICollection<OrderDetail> OrderDetails { get; set; }
    public ICollection<CartDetail> CartDetails { get; set; }
}