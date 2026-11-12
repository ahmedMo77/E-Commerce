using AutoMapper;
using ECommerce.Application.Common.Pagination;
using ECommerce.Application.DTOs.Orders;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;

namespace ECommerce.Application.Services
{
    public class StoreOrderService : IStoreOrderService
    {
        private readonly IStoreRepository _storeRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StoreOrderService(
            IOrderRepository orderRepository,
            IStoreRepository storeRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _storeRepository = storeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResult<StoreOrderDto>> GetMyStoreOrdersAsync(
            int managerId,
            PaginationParams pagination,
            CancellationToken ct = default)
        {
            PaginationValidator.Validate(pagination);

            var store = await _storeRepository.GetByManagerIdAsync(managerId, ct);

            if (store is null)
            {
                return new PagedResult<StoreOrderDto>
                {
                    Items = [],
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize,
                    TotalCount = 0
                };
            }

            var storeOrders = await _orderRepository.GetStoreOrdersAsync(
                store.Id,
                pagination.PageNumber,
                pagination.PageSize,
                ct);

            return new PagedResult<StoreOrderDto>
            {
                Items = _mapper.Map<IEnumerable<StoreOrderDto>>(
                    storeOrders.Items),
                PageNumber = storeOrders.PageNumber,
                PageSize = storeOrders.PageSize,
                TotalCount = storeOrders.TotalCount
            };
        }

        public async Task<StoreOrderDto?> GetStoreOrderAsync(
            int managerId,
            int storeOrderId,
            CancellationToken ct = default)
        {
            var store = await _storeRepository.GetByManagerIdAsync(
                managerId, ct);

            if (store is null)
                return null;

            var storeOrder = await _orderRepository.GetStoreOrderAsync(
                storeOrderId,
                store.Id,
                ct);

            if (storeOrder is null)
                return null;

            return _mapper.Map<StoreOrderDto>(storeOrder);
        }

        public async Task<StoreOrderDto> UpdateStatusAsync(
            int managerId,
            int storeOrderId,
            UpdateStoreOrderStatusDto dto,
            CancellationToken ct = default)
        {
            return await _unitOfWork.ExecuteInTransactionAsync(
                async token =>
                {
                    var store = await _storeRepository.GetByManagerIdAsync(
                        managerId, token);

                    if (store is null)
                        throw new KeyNotFoundException("Store not found.");

                    var storeOrder = await _orderRepository.GetStoreOrderAsync(
                        storeOrderId,
                        store.Id,
                        token);

                    if (storeOrder is null)
                        throw new KeyNotFoundException("Store order not found.");

                    ValidateStatusTransition(storeOrder.Status, dto.Status);

                    // Update StoreOrder status
                    storeOrder.Status = dto.Status;

                    // Get all StoreOrders belonging to the same Order
                    var storeOrders = await _orderRepository.GetStoreOrdersByOrderIdAsync(
                        storeOrder.OrderId,
                        token);

                    // Update the overall Order status
                    storeOrder.Order.Status = CalculateOrderStatus(storeOrders);

                    await _unitOfWork.SaveChangesAsync(token);

                    return _mapper.Map<StoreOrderDto>(storeOrder);

                }, ct);
        }

        // Helper methods for status validation and calculation
        #region Helpers Methods

        private static void ValidateStatusTransition(
            OrderStatus currentStatus,
            OrderStatus newStatus)
        {
            var isValidTransition =
                (currentStatus == OrderStatus.Pending &&
                 newStatus == OrderStatus.Processing)
                ||
                (currentStatus == OrderStatus.Processing &&
                 newStatus == OrderStatus.Shipped)
                ||
                (currentStatus == OrderStatus.Shipped &&
                 newStatus == OrderStatus.Delivered);

            if (!isValidTransition)
            {
                throw new InvalidOperationException(
                    $"Cannot change order status from {currentStatus} to {newStatus}.");
            }
        }

        private static OrderStatus CalculateOrderStatus(
            IEnumerable<StoreOrder> storeOrders)
        {
            var orders = storeOrders.ToList();

            if (orders.All(so => so.Status == OrderStatus.Pending))
                return OrderStatus.Pending;

            if (orders.All(so => so.Status == OrderStatus.Shipped ||
                                 so.Status == OrderStatus.Delivered))
                return OrderStatus.Shipped;

            return OrderStatus.Processing;
        }
        #endregion
    }
}
