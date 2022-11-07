using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Product.API.Data.Configurations;
using Product.API.Entity;

namespace Product.API.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
            
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new StoreConfiguration());
    }
    private DbSet<Store> Stores { get; set; }
    private DbSet<Entity.Product> Products { get; set; }
    private DbSet<ProductType> ProductTypes { get; set; }
    private DbSet<Image> Images { get; set; }
    
}