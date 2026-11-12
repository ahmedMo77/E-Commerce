using AutoMapper;
using ECommerce.Application.Common.Pagination;
using ECommerce.Application.DTOs.Product;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Interfaces.Services;

namespace ECommerce.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public ProductService(
            IProductRepository productRepository,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task<PagedResult<ProductDto>> GetActiveProductsAsync(
            PaginationParams pagination,
            CancellationToken ct = default)
        {    
            PaginationValidator.Validate(pagination);

            var result = await _productRepository.GetActiveProductsAsync(
                pagination.PageNumber,
                pagination.PageSize,
                ct);

            return new PagedResult<ProductDto>
            {
                Items = _mapper.Map<IEnumerable<ProductDto>>(result.Items),
                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount
            };
        }

        public async Task<ProductDto?> GetProductByIdAsync(
            int productId, 
            CancellationToken ct = default)
        {
            var product = await _productRepository
                .GetByIdAsync(productId, ct);

            if (product is null || !product.IsActive)
                return null;

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<PagedResult<ProductDto>> GetProductsByStoreAsync(
            int storeId, 
            PaginationParams pagination,
            CancellationToken ct = default)
        {
            PaginationValidator.Validate(pagination);

            var result = await _productRepository.GetProductsByStoreAsync(
                storeId,
                pagination.PageNumber,
                pagination.PageSize,
                ct);

            return new PagedResult<ProductDto>
            {
                Items = _mapper.Map<IEnumerable<ProductDto>>(result.Items),
                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount
            };
        }
    }
}
