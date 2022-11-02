using Microsoft.EntityFrameworkCore;
using Product.API.Entity;

namespace Product.API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        
    }
    private DbSet<Store> Stores { get; set; }
    private DbSet<Entity.Product> Products { get; set; }
    private DbSet<ProductType> ProductTypes { get; set; }
    private DbSet<Image> Images { get; set; }
    
}