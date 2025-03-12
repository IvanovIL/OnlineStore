using OnlineStore.Contracts.Product;

namespace OnlineStore.Contracts.Order
{
    /// <summary>
    /// Позиция заказа
    /// </summary>
    public sealed class OrderItemDto
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор заказа
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Идентификатор товара
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Признак удаление заказа
        /// </summary>
        public bool IsDeleted { get; set; } 

        /// <summary>
        /// Наименование товара
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Количество
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Цена
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Товар
        /// </summary>
        public ShortProductDto ShortProductDto { get; set; } = default!;

        /// <summary>
        /// Заказ
        /// </summary>
        public OrderDto OrderDto { get; set; } = default!;
    }
}
