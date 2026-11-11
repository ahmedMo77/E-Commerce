using AutoMapper;
using ECommerce.Application.DTOs.Carts;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CartService(
            ICartRepository cartRepository,
            IProductRepository productRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CartDto> GetCartAsync(
            int customerId, 
            CancellationToken ct = default)
        {
            var cart = await GetOrCreateCartAsync(customerId, ct);
            return _mapper.Map<CartDto>(cart);
        }

        public async Task<CartDto> AddToCartAsync(
            int customerId,
            int productId,
            int quantity,
            CancellationToken ct = default)
        {
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity));

            var product = await _productRepository.GetByIdAsync(productId, ct);

            if (product is null)
                throw new KeyNotFoundException("Product not found.");

            if (!product.IsActive)
                throw new InvalidOperationException("Product is not available.");

            var cart = await GetOrCreateCartAsync(customerId, ct);

            var existingItem = cart.CartItems
                .FirstOrDefault(ci => ci.ProductId == productId);

            if (existingItem is not null)
            {
                var newQuantity = existingItem.Quantity + quantity;

                if (newQuantity > product.Quantity)
                    throw new InvalidOperationException("Insufficient stock.");

                existingItem.Quantity = newQuantity;
            }
            else
            {
                cart.CartItems.Add(new CartItem
                {
                    ProductId = productId,
                    Product = product,
                    Quantity = quantity
                });
            }

            await _unitOfWork.SaveChangesAsync(ct);

            return _mapper.Map<CartDto>(cart);
        }

        public async Task<CartDto> UpdateCartItemQuantityAsync(
            int customerId, 
            int productId, 
            int quantity, 
            CancellationToken ct = default)
        {
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity));

            var cart = await GetCartOrThrowAsync(customerId, ct);
            
            var cartItem = GetCartItemOrThrow(cart, productId);

            if (!cartItem.Product.IsActive)
                throw new InvalidOperationException("Product is not available.");

            if (quantity > cartItem.Product.Quantity)
                throw new InvalidOperationException("Insufficient stock.");

            cartItem.Quantity = quantity;

            await _unitOfWork.SaveChangesAsync(ct);

            return _mapper.Map<CartDto>(cart);
        }

        public async Task<CartDto> RemoveCartItemAsync(
            int customerId, 
            int productId,
            CancellationToken ct = default)
        {
            var cart = await GetCartOrThrowAsync(customerId, ct);

            var cartItem = GetCartItemOrThrow(cart, productId);
            
            cart.CartItems.Remove(cartItem);

            await _unitOfWork.SaveChangesAsync(ct);

            return _mapper.Map<CartDto>(cart);
        }

        // Helper methods to encapsulate common logic for retrieving or creating a cart and getting cart items.
        #region Helper methods
        private async Task<Cart> GetOrCreateCartAsync(
            int customerId,
            CancellationToken ct)
        {
            var cart = await _cartRepository
                .GetCartByCustomerIdAsync(customerId, ct);

            if (cart is not null)
                return cart;

            cart = new Cart
            {
                CustomerId = customerId
            };

            await _cartRepository.AddAsync(cart, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return cart;
        }

        private async Task<Cart> GetCartOrThrowAsync(
            int customerId,
            CancellationToken ct)
        {
            var cart = await _cartRepository
                .GetCartByCustomerIdAsync(customerId, ct);

            return cart ?? throw new KeyNotFoundException("Cart not found.");
        }

        private CartItem GetCartItemOrThrow(Cart cart, int productId)
        {
            return cart.CartItems.FirstOrDefault(
                ci => ci.ProductId == productId)
                ?? throw new KeyNotFoundException("Product not found in cart.");
        }
        #endregion
    }
}
