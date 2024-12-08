

namespace OnlineStore.Contracts.Order
{
    public sealed class OrderDto
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string userName { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Адрес доставки продукта пользователю
        /// </summary>
        public string addressUser { get; set; }

        /// <summary>
        /// Номер телефона пользователя
        /// </summary>
        public string numberPhoneUser { get; set; }

        /// <summary>
        /// Дата заказа
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// Сумма заказа
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Идентификатор статуса заказа
        /// </summary>
        public int OrderStatusId { get; set; }


        /// <summary>
        /// Позиции заказа
        /// </summary>
        //public ICollection<OrderItem> Items { get; set; } = [];
    }
}
