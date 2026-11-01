using ECommerce.Application.Common.Models;
using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}