using ECommerce.Application.Common.Pagination;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces.Repositories
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order?> GetOrderDetailsAsync(
            int orderId,
            CancellationToken ct = default);

        Task<PagedResult<Order>> GetOrdersByCustomerIdAsync(
            int customerId,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default);

        Task<PagedResult<StoreOrder>> GetStoreOrdersAsync(
            int storeId,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default);

        Task<StoreOrder?> GetStoreOrderAsync(
            int storeOrderId,
            int storeId,
            CancellationToken ct = default);

        Task<List<StoreOrder>> GetStoreOrdersByOrderIdAsync(
            int orderId,
            CancellationToken ct = default);

        Task<Order?> GetOrderForCancellationAsync(
            int orderId,
            CancellationToken ct = default);
    }
}
