using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Product.API.Entity;

namespace Product.API.Data.Configurations;

public class StoreConfiguration : IEntityTypeConfiguration<Entity.Store>
{
    public void Configure(EntityTypeBuilder<Store> builder)
    {
        builder.HasOne(x => x.ApplicationUser)
            .WithOne(x => x.Store)
            .HasForeignKey<Store>(x => x.UserId);
    }
}