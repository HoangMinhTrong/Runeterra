using Microsoft.EntityFrameworkCore;
using Product.API.Entity;

namespace Product.API.Data.DataSeeding;

public static class SeedData
{
    public static void  Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductType>().HasData(
            new ProductType
            {
                Id = 1,
                Name = "Manga"
            }
        );
    }
}