using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Product.API.Data.Configurations;
using Product.API.Data.DataSeeding;
using Product.API.Entity;

namespace Product.API.Data;

public class ApplicationDbContext : DbContext
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
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        modelBuilder.Seed();
    }
    public DbSet<Store> Stores { get; set; }
    public DbSet<Entity.Product> Products { get; set; }
    public DbSet<ProductType> ProductTypes { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartDetail> CartDetails {get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<OrderType> OrderTypes { get; set; }


}