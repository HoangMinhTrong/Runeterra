namespace Product.API.Dtos.Responses;

public class StoreInfoResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public bool Status { get; set; }
    public string Description { get; set; }
    public string UserId { get; set; }
}