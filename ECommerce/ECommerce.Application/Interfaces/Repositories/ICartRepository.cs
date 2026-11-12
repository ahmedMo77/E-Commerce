using ECommerce.Domain.Entities;

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
