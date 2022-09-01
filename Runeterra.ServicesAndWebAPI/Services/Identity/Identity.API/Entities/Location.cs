namespace Identity.API.Entities;

public class Location
{
    public int Id { get; set; }
    public string BuildingNo { get; set; }
    public string ApartmentNo { get; set; }
    public IEnumerable<ApplicationUser> ApplicationUsers { get; set; }
}