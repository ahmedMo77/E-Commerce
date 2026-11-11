using ECommerce.Application.DTOs.Carts;

namespace ECommerce.Application.Interfaces.Services
{
    public interface ICartService
    {
        Task<CartDto> GetCartAsync(
            int customerId,
            CancellationToken ct = default);

        Task<CartDto> AddToCartAsync(
            int customerId,
            int productId,
            int quantity,
            CancellationToken ct = default);

        Task<CartDto> UpdateCartItemQuantityAsync(
            int customerId,
            int productId,
            int quantity,
            CancellationToken ct = default);

        Task<CartDto> RemoveCartItemAsync(
            int customerId,
            int productId,
            CancellationToken ct = default);
    }
}
