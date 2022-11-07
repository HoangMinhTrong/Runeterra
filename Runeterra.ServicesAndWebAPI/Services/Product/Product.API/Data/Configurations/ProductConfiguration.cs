using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Product.API.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Entity.Product>

{
    public void Configure(EntityTypeBuilder<Entity.Product> builder)
    {
        builder.HasOne(x => x.ProductType)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.ProductTypeId);
        builder.HasOne(x => x.Store)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.StoreId);
        builder.HasOne(x => x.Image)
            .WithOne(x => x.Product)
            .HasForeignKey<Entity.Product>(x => x.ImageId);
    }
}