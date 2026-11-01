using ECommerce.Application.Common.Models;
using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces.Repositories
{
    public interface IOrderRepository
        : IGenericRepository<Order>
    {
        Task<Order?> GetOrderDetailsAsync(
            int orderId,
            CancellationToken ct = default);
        Task<PagedResult<Order>> GetOrdersByCustomerIdAsync(
            int customerId,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default);
    }
}
