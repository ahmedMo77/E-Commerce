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
                .Where(o => o.CustomerId == customerId);

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
    }
}
