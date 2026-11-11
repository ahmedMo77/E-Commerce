using ECommerce.Application.Interfaces.Messaging;
using MassTransit;

namespace ECommerce.Infrastructure.Messaging
{
    public class MassTransitEventPublisher : IEventPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public MassTransitEventPublisher(
            IPublishEndpoint publishEndpoint)
            => _publishEndpoint = publishEndpoint;

        public async Task PublishAsync<T>(
            T message,
            CancellationToken ct = default)
            where T : class
        {
            await _publishEndpoint.Publish(message, ct);
        }
    }
}
