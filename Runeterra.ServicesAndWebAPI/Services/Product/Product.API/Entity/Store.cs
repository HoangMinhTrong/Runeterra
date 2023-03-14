using System.ComponentModel.DataAnnotations;

namespace Product.API.Entity;

public class Store
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public bool Status { get; set; }
    public string Description { get; set; }
    public string UserId { get; set; }
    public ICollection<Product> Products { get; set; }

}