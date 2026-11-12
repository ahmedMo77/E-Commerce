using ECommerce.Application.Common.Pagination;
using ECommerce.Application.DTOs.Orders;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces.Services
{
    public interface IOrderService
    {
        Task<OrderDto> CheckoutAsync(
            int customerId,
            CancellationToken ct = default);

        Task<PagedResult<OrderDto>> GetMyOrdersAsync(
            int customerId,
            PaginationParams pagination,
            CancellationToken ct = default);

        Task<OrderDto?> GetOrderDetailsAsync(
            int customerId,
            int orderId,
            CancellationToken ct = default);

        Task<OrderDto?> CancelOrderAsync(
            int customerId,
            int orderId,
            CancellationToken ct = default);
    }
}
