using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Product.API.Entity;

namespace Product.API.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasMany(x => x.OrderDetails)
            .WithOne(x => x.Order)
            .HasForeignKey(x => x.orderId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(x => x.OrderType)
            .WithMany(x => x.Orders)
            .HasForeignKey(x => x.orderTypeId);
        builder.HasOne(x => x.DeliveryAddress)
            .WithMany(x => x.Orders)
            .HasForeignKey(x => x.DeliveryId);
    }
}