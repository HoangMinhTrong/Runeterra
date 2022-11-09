using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace UserManagement.API.Entity;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string BuildingNo { get; set; }

    [NotMapped] 
    public IEnumerable<string> Role { get; set; }
}