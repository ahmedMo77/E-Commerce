using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Data.Configurations
{
    public class OrderConfiguration
        : IEntityTypeConfiguration<Order>
    {
        public void Configure(
            EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders", table =>
            {
                table.HasCheckConstraint(
                    "CK_Orders_TotalAmount_NonNegative",
                    "[TotalAmount] >= 0");
            });

            builder.HasKey(x => x.Id);

            builder.Property(o => o.CreatedAt)
                .IsRequired();

            builder.Property(o => o.TotalAmount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.Status)
                .HasConversion<string>()
                .IsRequired();

            builder.HasOne(o => o.Customer)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.CustomerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(o => o.CustomerId);
        }
    }
}
