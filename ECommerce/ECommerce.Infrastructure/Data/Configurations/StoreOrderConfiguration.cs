using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Data.Configurations
{
    public class StoreOrderConfiguration : IEntityTypeConfiguration<StoreOrder>
    {
        public void Configure(EntityTypeBuilder<StoreOrder> builder)
        {
            builder.HasKey(so => so.Id);

            builder.Property(so => so.Status)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(so => so.CreatedAt)
                .IsRequired();

            // Store Relationship
            builder.HasOne(so => so.Store)
                .WithMany(s => s.StoreOrders)
                .HasForeignKey(so => so.StoreId)
                .OnDelete(DeleteBehavior.Restrict);

            // Order Relationship
            builder.HasOne(so => so.Order)
                .WithMany(o => o.StoreOrders)
                .HasForeignKey(so => so.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Prevent duplicate StoreOrders for the same Order and Store
            builder.HasIndex(so => new { so.OrderId, so.StoreId })
                .IsUnique();

            builder.HasIndex(so => so.StoreId);
        }
    }
}
