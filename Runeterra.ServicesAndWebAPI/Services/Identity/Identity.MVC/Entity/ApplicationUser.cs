﻿using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Identity.MVC.Entity;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string BuildingNo { get; set; }
    // public virtual Store Store { get; set; }
    
    [NotMapped] 
    public string Role { get; set; }
}