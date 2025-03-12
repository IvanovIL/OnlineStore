
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.Domain.Entities
{
    /// <summary>
    /// Товар корзины
    /// </summary>
    public sealed class CartProduct
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор продукта
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Наименование продукта
        /// </summary>
        public string productName { get; set; }

        /// <summary>
        /// Цена
        /// </summary>
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        /// <summary>
        /// Количество
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Идентфикатор корзины
        /// </summary>
        public int CartId { get; set; }

        /// <summary>
        /// Корзина
        /// </summary>
        public Cart Cart { get; set; }

        /// <summary>
        /// Товар
        /// </summary>
        public Product Product { get; set; }

    }
}
