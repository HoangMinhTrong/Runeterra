using Microsoft.EntityFrameworkCore;
using Product.API.Entity;

namespace Product.API.Data.DataSeeding;

public static class SeedData
{
    public static void  Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Image>().HasData(
            new Image
            {
                Id = "1",
                Url = "https://media-cdn-v2.laodong.vn/Storage/NewsPortal/2020/8/21/829850/Bat-Cuoi-Truoc-Nhung-07.jpg"
            }
        );
        modelBuilder.Entity<ProductType>().HasData(
            new ProductType
            {
                Id = "1",
                Name = "Manga"
            }
        );
    }
}