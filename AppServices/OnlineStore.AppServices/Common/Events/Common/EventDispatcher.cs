using Microsoft.Extensions.DependencyInjection;
using OnlineStore.Domain.Events;

namespace OnlineStore.AppServices.Common.Events.Common
{
    public sealed class EventDispatcher : IEventDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public EventDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task DispatchAsync(IDomainEvent domainEvent)
        {
            var eventType = domainEvent.GetType();
            var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(eventType);

            var handlers = _serviceProvider.GetServices(handlerType);

            foreach (var handler in handlers)
            {
                if (handler == null)
                {
                    continue;
                }

                await ((IDomainEventHandler)handler).HandleAsync(domainEvent);
            }
        }
    }
}
