using OnlineStore.AppServices.Common.Events.Common;
using OnlineStore.AppServices.Common.NotificationServices;
using OnlineStore.Contracts.Enum;
using OnlineStore.Domain.Events;

namespace OnlineStore.AppServices.Common.Events.Handlers
{
    /// <summary>
    /// Обработчик события создания товара
    /// </summary>
    public sealed class AddProductEventHandler : IDomainEventHandler<AddProductEvent>
    {
        private readonly INotificationService _notificationService;


        public AddProductEventHandler(
            INotificationService notificationService)
        {
            _notificationService = notificationService;

        }

        /// <inheritdoc/>
        public Task HandleAsync(AddProductEvent @event)
        {
            //return _messageQueueService.SendMessageAsync(new object(), CancellationToken.None);
            return _notificationService.SendNotificationAsync(new Contracts.Notifications.NotificationDto
            {
                Theme = $"Добавлен новый товар - {@event.productName}",
                Email = "email@email.com",
                Text = $"Добавлен новый товар - {@event.productName}",
                NotificationChannels = [NotificationChannelEnum.Email, NotificationChannelEnum.Telegram]
            }, CancellationToken.None);
        }

        public Task HandleAsync(IDomainEvent @event)
        {
            return HandleAsync((AddProductEvent)@event);
        }
    }
}
