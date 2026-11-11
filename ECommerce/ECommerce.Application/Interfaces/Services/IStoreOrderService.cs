using ECommerce.Application.Common.DTOs;
using ECommerce.Application.Common.Models;
using ECommerce.Application.DTOs.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces.Services
{
    public interface IStoreOrderService
    {
        Task<PagedResult<StoreOrderDto>> GetMyStoreOrdersAsync(
            int managerId,
            PaginationParams pagination,
            CancellationToken ct = default);

        Task<StoreOrderDto?> GetStoreOrderAsync(
            int managerId,
            int storeOrderId,
            CancellationToken ct = default);

        Task<StoreOrderDto> UpdateStatusAsync(
            int managerId,
            int storeOrderId,
            UpdateStoreOrderStatusDto dto,
            CancellationToken ct = default);
    }
}
