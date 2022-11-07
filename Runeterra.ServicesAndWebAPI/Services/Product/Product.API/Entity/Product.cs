using System.ComponentModel.DataAnnotations;

namespace Product.API.Entity;

public class Product
{
    [Key]
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public bool Status { get; set; }
    public int Quantity { get; set; }
    public string ImageId { get; set; }
    public string ProductTypeId { get; set; }
    public string StoreId { get; set; }
    public Store Store { get; set; }
    public Image Image { get; set; }
    public ProductType ProductType { get; set; }
}