using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Data.Configurations
{
    public class ProductConfiguration
        : IEntityTypeConfiguration<Product>
    {
        public void Configure(
            EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products", table =>
            {
                table.HasCheckConstraint(
                    "CK_Products_Price_NonNegative",
                    "[Price] >= 0");

                table.HasCheckConstraint(
                    "CK_Products_Quantity_NonNegative",
                    "[Quantity] >= 0");
            });

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Description)
                .HasMaxLength(500);

            builder.Property(p => p.Price)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(p => p.IsActive)
                .HasDefaultValue(true)
                .IsRequired();

            builder.HasOne(p => p.Store)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.StoreId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}