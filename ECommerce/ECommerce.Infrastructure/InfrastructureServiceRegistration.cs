using ECommerce.Application.Interfaces.Messaging;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Infrastructure.Data;
using ECommerce.Infrastructure.Messaging;
using ECommerce.Infrastructure.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace ECommerce.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Database
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));

            // Generic Repository
            services.AddScoped(
                typeof(IGenericRepository<>),
                typeof(GenericRepository<>));

            // Specific Repositories
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IStoreRepository, StoreRepository>();

            // Unit Of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Event Publisher
            services.AddScoped<IEventPublisher, MassTransitEventPublisher>();

            var rabbitMqHost = configuration["RabbitMQ:Host"]
                ?? throw new InvalidOperationException("Configuration value 'RabbitMQ:Host' is required.");
            var rabbitMqUsername = configuration["RabbitMQ:Username"]
                ?? throw new InvalidOperationException("Configuration value 'RabbitMQ:Username' is required.");
            var rabbitMqPassword = configuration["RabbitMQ:Password"]
                ?? throw new InvalidOperationException("Configuration value 'RabbitMQ:Password' is required.");

            // MassTransit with RabbitMQ
            services.AddMassTransit(x =>
            {
                x.AddEntityFrameworkOutbox<AppDbContext>(options =>
                {
                    options.UseSqlServer();
                    options.UseBusOutbox();
                });

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(
                        rabbitMqHost,
                        h =>
                        {
                            h.Username(rabbitMqUsername);
                            h.Password(rabbitMqPassword);
                        });

                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}
