using AutoMapper;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Application.Extensions
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            // AutoMapper
            services.AddAutoMapper(typeof(ApplicationServiceRegistration).Assembly);

            // Services
            services.AddScoped<IStoreService, StoreService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IStoreOrderService, StoreOrderService>();

            return services;
        }
    }
}