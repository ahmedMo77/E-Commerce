using ECommerce.Application.Common.Pagination;
using ECommerce.Application.DTOs.Product;

namespace ECommerce.Application.Interfaces.Services
{
    public interface IProductService
    {
        Task<PagedResult<ProductDto>> GetActiveProductsAsync(
            PaginationParams pagination,
            CancellationToken ct = default);

        Task<PagedResult<ProductDto>> GetProductsByStoreAsync(
            int storeId,
            PaginationParams pagination,
            CancellationToken ct = default);

        Task<ProductDto?> GetProductByIdAsync(
            int productId,
            CancellationToken ct = default);
    }
}
