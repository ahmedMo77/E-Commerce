using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Data.Configurations
{
    public class CartItemConfiguration
        : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(
            EntityTypeBuilder<CartItem> builder)
        {
            builder.ToTable("CartItems", table =>
            {
                table.HasCheckConstraint(
                    "CK_CartItems_Quantity_Positive",
                    "[Quantity] > 0");
            });

            builder.HasKey(x => x.Id);

            builder.HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ci => ci.Product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(ci => ci.ProductId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(ci => new {
                ci.CartId, ci.ProductId })
                .IsUnique();
        }
    }
}
