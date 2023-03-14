﻿using System.ComponentModel.DataAnnotations;

namespace Product.API.Entity;

public class Cart
{
    [Key]
    public int id { get; set; }
    public string userId { get; set; }
    public DateTime createAt { get; set; }
    public ICollection<CartDetail> CartDetails { get; set; }
}