using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories
{
    public class CartRepository
        : GenericRepository<Cart>, ICartRepository
    {
        public CartRepository(AppDbContext context) 
            : base(context)
        { 
        }

        public async Task<Cart?> GetCartByCustomerIdAsync(
            int customerId,
            CancellationToken ct = default)
        {
            return await _dbSet
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(
                    c => c.CustomerId == customerId,
                    ct);
        }

        public async Task<CartItem?> GetCartItemAsync(
            int cartId,
            int productId, 
            CancellationToken ct = default)
        {
            return await _context.CartItems
                .FirstOrDefaultAsync(
                    ci => ci.CartId == cartId &&
                          ci.ProductId == productId,
                    ct);
        }
    }
}
