using System.ComponentModel.DataAnnotations;

namespace Product.API.Dtos;

public class StoreDto
{
    [Key]
    public string Id { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public bool Status { get; set; }
    public string Description { get; set; }
    public string UserId { get; set; }
}