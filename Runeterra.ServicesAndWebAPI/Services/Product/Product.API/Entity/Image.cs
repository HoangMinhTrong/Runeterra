using System.ComponentModel.DataAnnotations;

namespace Product.API.Entity;

public class Image
{
    [Key]
    public string Id { get; set; }
    public string Url { get; set; }
    public Product Product { get; set; }
}