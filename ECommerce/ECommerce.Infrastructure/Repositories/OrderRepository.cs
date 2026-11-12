using ECommerce.Application.Common.Pagination;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories
{
    public class OrderRepository :
        GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) 
            : base(context) 
        {
        }

        public async Task<Order?> GetOrderDetailsAsync(
            int orderId, 
            CancellationToken ct = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId, ct);
        }

        public async Task<PagedResult<Order>> GetOrdersByCustomerIdAsync(
            int customerId, 
            int pageNumber, 
            int pageSize, 
            CancellationToken ct = default)
        {
            var query = _dbSet
                .AsNoTracking()
                .Where(o => o.CustomerId == customerId)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product);

            var totalCount = await query.CountAsync(ct);

            var orders = await query
                .OrderByDescending(o => o.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return new PagedResult<Order>
            {
                Items = orders,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<StoreOrder?> GetStoreOrderAsync(
            int storeOrderId,
            int storeId,
            CancellationToken ct = default)
        {
            return await _context.Set<StoreOrder>()
                .Include(so => so.Order)
                    .ThenInclude(o => o.OrderItems
                        .Where(oi => oi.Product.StoreId == storeId))
                        .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(
                    so => so.Id == storeOrderId && so.StoreId == storeId,
                    ct);
        }

        public async Task<PagedResult<StoreOrder>> GetStoreOrdersAsync(
            int storeId,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default)
        {
            var query = _context.Set<StoreOrder>()
                .AsNoTracking()
                .Where(so => so.StoreId == storeId);

            var totalCount = await query.CountAsync(ct);

            var storeOrders = await query
                .OrderByDescending(so => so.Id)
                .Include(so => so.Order)
                    .ThenInclude(o => o.OrderItems
                        .Where(oi => oi.Product.StoreId == storeId))
                        .ThenInclude(oi => oi.Product)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return new PagedResult<StoreOrder>
            {
                Items = storeOrders,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<List<StoreOrder>> GetStoreOrdersByOrderIdAsync(
            int orderId,
            CancellationToken ct = default)
        {
            return await _context.Set<StoreOrder>()
                .Where(so => so.OrderId == orderId)
                .ToListAsync(ct);
        }

        public async Task<Order?> GetOrderForCancellationAsync(
            int orderId,
            CancellationToken ct = default)
        {
            return await _dbSet
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.StoreOrders)
                .FirstOrDefaultAsync(o => o.Id == orderId, ct);
        }
    }
}
