using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Data.Configurations
{
    public class CartConfiguration
        : IEntityTypeConfiguration<Cart>
    {
        public void Configure(
            EntityTypeBuilder<Cart> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasOne(c => c.Customer)
                .WithOne(u => u.Cart)
                .HasForeignKey<Cart>(c => c.CustomerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.CartItems)
                .WithOne(ci => ci.Cart)
                .HasForeignKey(ci => ci.CartId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}