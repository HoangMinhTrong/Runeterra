using System.ComponentModel.DataAnnotations;

namespace Product.API.Dtos;

public class CreateProductRequest
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
}