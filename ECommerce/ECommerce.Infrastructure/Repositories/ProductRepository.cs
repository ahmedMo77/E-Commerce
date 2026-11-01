using ECommerce.Application.Common.Models;
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
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context)
            : base(context)
        {
        }

        public async Task<PagedResult<Product>> GetActiveProductsAsync(
            int pageNumber,
            int pageSize,
            CancellationToken ct = default)
        {
            var query = _dbSet
                .AsNoTracking()
                .Where(p => p.IsActive);

            var totalCount = await query.CountAsync(ct);

            var products = await query
                .OrderBy(p => p.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return new PagedResult<Product>
            {
                Items = products,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        public async Task<PagedResult<Product>> GetProductsByStoreAsync(
            int storeId,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default)
        {
            var query = _dbSet
                .AsNoTracking()
                .Where(p => p.StoreId == storeId && p.IsActive);

            var totalCount = await query.CountAsync(ct);

            var products = await query
                .OrderBy(p => p.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return new PagedResult<Product>
            {
                Items = products,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }
        public async Task<IEnumerable<Product>> GetProductsByIdsAsync(
            IEnumerable<int> ids,
            CancellationToken ct = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(p => ids.Contains(p.Id))
                .ToListAsync(ct);
        }
    }
}
