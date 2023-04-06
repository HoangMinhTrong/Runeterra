namespace Product.API.Dtos.Order.Requests;

public class ConfirmCheckoutRequest
{
    public string Address { get; set; }
    public string BuildingNo { get; set; }
    public string ApartmentNo { get; set; }
    public int OrderTypeId { get; set; }
}