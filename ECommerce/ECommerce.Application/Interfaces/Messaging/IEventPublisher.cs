using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces.Messaging
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(
            T message,
            CancellationToken ct = default)
            where T : class;
    }
}
