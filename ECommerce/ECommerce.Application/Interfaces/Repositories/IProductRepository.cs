using ECommerce.Application.Common.Pagination;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces.Repositories
{
    public interface IProductRepository
        : IGenericRepository<Product>
    {
        Task<PagedResult<Product>> GetActiveProductsAsync(
            int pageNumber,
            int pageSize,
            CancellationToken ct = default);

        Task<PagedResult<Product>> GetProductsByStoreAsync(
            int storeId,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default);

        Task<IEnumerable<Product>> GetProductsByIdsAsync(
            IEnumerable<int> ids,
            CancellationToken ct = default);

        Task<bool> TryDecreaseStockAsync(
            int productId,
            int quantity,
            CancellationToken ct = default);

        Task<bool> TryIncreaseStockAsync(
            int productId,
            int quantity,
            CancellationToken ct = default);
    }
}