
namespace OnlineStore.Domain.Events
{
    /// <summary>
    /// Событие добавление нового заказа на продукт
    /// </summary>
    public sealed class AddOrderProductsEvent : IDomainEvent
    {
        public DateTime eventDate { get; set; }

        /// <summary>
        /// Наименование продукта
        /// </summary>
        public string productName { get; set; } = default!;

        /// <summary>
        /// Email покупателя
        /// </summary>
        public string Email { get; set; }

    }
}
