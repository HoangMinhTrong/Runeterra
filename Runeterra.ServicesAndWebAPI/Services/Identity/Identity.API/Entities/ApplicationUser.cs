using Microsoft.AspNetCore.Identity;

namespace Identity.API.Entities;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int LocationId { get; set; }
    public Location Location { get; set; }
}