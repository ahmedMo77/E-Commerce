using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Data.Configurations
{
    public class AppUserConfiguration
        : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(
            EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(u => u.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasOne(u => u.Store)
                .WithOne(s => s.Manager)
                .HasForeignKey<Store>(s => s.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(u => u.Cart)
                .WithOne(c => c.Customer)
                .HasForeignKey<Cart>(c => c.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}