using AutoMapper;
using ECommerce.Application.DTOs;
using ECommerce.Application.DTOs.Carts;
using ECommerce.Application.DTOs.Orders;
using ECommerce.Application.DTOs.Store;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Product mappings
            CreateMap<Product, ProductDto>();

            // Cart mappings
            CreateMap<CartItem, CartItemDto>()
                .ForMember(
                    dest => dest.ProductName,
                    opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(
                    dest => dest.UnitPrice,
                    opt => opt.MapFrom(src => src.Product.Price));

            CreateMap<Cart, CartDto>();

            // Order mappings
            CreateMap<Order, OrderDto>()
                .ForMember(
                    dest => dest.Items,
                    opt => opt.MapFrom(src => src.OrderItems));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(
                    dest => dest.ProductName,
                    opt => opt.MapFrom(src => src.Product.Name));

            CreateMap<StoreOrder, StoreOrderDto>()
                .ForMember(
                    dest => dest.Items,
                    opt => opt.MapFrom(src => src.Order.OrderItems));


            // Store mappings
            CreateMap<Store, StoreDto>();

            CreateMap<CreateStoreDto, Store>();

            CreateMap<UpdateStoreDto, Store>();
        }
    }
}
