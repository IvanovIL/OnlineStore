using OnlineStore.Domain.Events;

namespace OnlineStore.AppServices.Common.Events.Common
{
    /// <summary>
    /// Интерфейс диспетчера доменных  событий
    /// </summary>
    public interface IEventDispatcher
    {
        /// <summary>
        /// Выполняет обработку доманных событий
        /// </summary>
        /// <param name="domainEvent"></param>
        Task DispatchAsync(IDomainEvent domainEvent);

    }
}
