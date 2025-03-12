using OnlineStore.AppServices.Common.Events.Common;
using OnlineStore.AppServices.Common.NotificationServices;
using OnlineStore.Contracts.Enum;
using OnlineStore.Domain.Events;

namespace OnlineStore.AppServices.Common.Events.Handlers
{
    /// <summary>
    /// Обработчик события добавления нового заказа на продукт
    /// </summary>
    public sealed class OrderProductsEventHandler : IDomainEventHandler<AddOrderProductsEvent>
    {
        private readonly INotificationService _notificationService;

        public OrderProductsEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public Task HandleAsync(AddOrderProductsEvent @event)
        {
            return _notificationService.SendNotificationAsync(new Contracts.Notifications.NotificationDto
            {
                Theme = "Онлайн магазин",
                Email = @event.Email,
                Text = @event.productName,
                NotificationChannels = [NotificationChannelEnum.Email, NotificationChannelEnum.Telegram]
            }, CancellationToken.None);
        }

        public Task HandleAsync(IDomainEvent @event)
        {
            return HandleAsync((AddOrderProductsEvent)@event);
        }
    }
}
