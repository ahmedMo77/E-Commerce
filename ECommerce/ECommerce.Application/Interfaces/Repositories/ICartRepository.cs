using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces.Repositories
{
    public interface ICartRepository
        : IGenericRepository<Cart>
    {
        Task<Cart?> GetCartByCustomerIdAsync(
            int customerId,
            CancellationToken ct = default);

        Task<CartItem?> GetCartItemAsync(
            int cartId, 
            int productId,
            CancellationToken ct = default);
    }
}
