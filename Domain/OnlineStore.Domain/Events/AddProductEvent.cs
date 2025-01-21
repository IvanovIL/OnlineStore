

namespace OnlineStore.Domain.Events
{
    /// <summary>
    /// Событие добавление нового товара
    /// </summary>
    public sealed class AddProductEvent : IDomainEvent
    {
        public DateTime eventDate { get; set; }

        /// <summary>
        /// Наименование товара
        /// </summary>
        public string productName { get; set; } = default!;


        public string Email { get; set; }

    }
}
