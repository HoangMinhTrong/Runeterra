using System.ComponentModel.DataAnnotations;

namespace Product.API.Entity;

public class ProductType
{
    [Key]
    public string Id { get; set; }
    public string Name { get; set; }
    public ICollection<Product> Products { get; set; }
}