using ECommerce.Application.Common.DTOs;
using ECommerce.Application.Common.Events;
using ECommerce.Application.Common.Helpers;
using ECommerce.Application.Common.Models;
using ECommerce.Application.DTOs.Orders;
using ECommerce.Application.Interfaces.Messaging;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities;
using AutoMapper;


namespace ECommerce.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(
            IOrderRepository orderRepository,
            ICartRepository cartRepository,
            IProductRepository productRepository,
            IEventPublisher eventPublisher,
            IUnitOfWork unitOfWork, IMapper mapper)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _eventPublisher = eventPublisher;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<OrderDto> CheckoutAsync(
            int customerId,
            CancellationToken ct = default)
        {
            return await _unitOfWork.ExecuteInTransactionAsync(
                async token =>
                {
                    // Retrieve the cart for the customer
                    var cart = await GetCartOrThrowAsync(customerId, token);

                    // Decrease stock for each product in the cart
                    await DecreaseStockAsync(cart.CartItems, token);

                    // Create the order from the cart items and save it to the database
                    var order = CreateOrder(customerId, cart.CartItems);
                    await _orderRepository.AddAsync(order, token);

                    cart.CartItems.Clear();

                    await _unitOfWork.SaveChangesAsync(token);

                    // Publish the OrderCreatedEvent to notify other services
                    await PublishOrderCreatedEventAsync(order, token);

                    await _unitOfWork.SaveChangesAsync(token);

                    return _mapper.Map<OrderDto>(order);
                }, ct);
        }

        public async Task<PagedResult<OrderDto>> GetMyOrdersAsync(
            int customerId,
            PaginationParams pagination,
            CancellationToken ct = default)
        {
            PaginationValidator.Validate(pagination);

            var orders = await _orderRepository.GetOrdersByCustomerIdAsync(
                customerId,
                pagination.PageNumber,
                pagination.PageSize,
                ct);

            return new PagedResult<OrderDto>
            {
                Items = _mapper.Map<IEnumerable<OrderDto>>(orders.Items),
                PageNumber = orders.PageNumber,
                PageSize = orders.PageSize,
                TotalCount = orders.TotalCount
            };
        }

        public async Task<OrderDto?> GetOrderDetailsAsync(
            int customerId,
            int orderId,
            CancellationToken ct = default)
        {
            var order = await _orderRepository.GetOrderDetailsAsync(
                orderId, ct);

            if (order is null || order.CustomerId != customerId)
                return null;

            return _mapper.Map<OrderDto>(order);
        }

        //Helper methods to encapsulate common logic for retrieving the cart,
        // decreasing stock, creating an order, and publishing events.
        #region
        private async Task<Cart> GetCartOrThrowAsync(
            int customerId,
            CancellationToken ct)
        {
            var cart = await _cartRepository
                .GetCartByCustomerIdAsync(customerId, ct);

            if (cart is null || !cart.CartItems.Any())
                throw new InvalidOperationException("Your cart is empty.");

            return cart;
        }
        private async Task DecreaseStockAsync(
            IEnumerable<CartItem> cartItems,
            CancellationToken ct)
        {
            foreach (var item in cartItems)
            {
                var success = await _productRepository.TryDecreaseStockAsync(
                    item.ProductId,
                    item.Quantity,
                    ct);

                if (!success)
                    throw new InvalidOperationException(
                        $"Insufficient stock for product: {item.Product.Name}");
            }
        }
        private Order CreateOrder(
            int customerId,
            IEnumerable<CartItem> cartItems)
        {
            var order = new Order
            {
                CustomerId = customerId,
                CreatedAt = DateTime.UtcNow,
                OrderItems = cartItems.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    Product = item.Product,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product.Price
                }).ToList()
            };

            order.TotalAmount = order.OrderItems.Sum(
                item => item.Quantity * item.UnitPrice);

            return order;
        }

        private async Task PublishOrderCreatedEventAsync(
            Order order,
            CancellationToken ct)
        {
            var storeIds = order.OrderItems
                .Select(item => item.Product.StoreId)
                .Distinct()
                .ToList();

            var orderCreatedEvent = new OrderCreatedEvent(
                order.Id,
                storeIds);

            await _eventPublisher.PublishAsync(
                orderCreatedEvent,
                ct);
        }
        #endregion
    }
}
