

using OnlineStore.AppServices.Common;
using OnlineStore.Contracts.Carts;
using OnlineStore.Contracts.Common;
using OnlineStore.Contracts.Order;
using OnlineStore.Contracts.Product;
using OnlineStore.Domain.Entities;


namespace OnlineStore.AppServices.Orders.Services
{
    public interface IOrderServices 
    {
        /// <summary>
        /// Возвращает список заказов
        /// </summary>
        /// <param name="cancellation">Токен отмены операции</param>
        Task<OrderDto> GetOrderAsync(CancellationToken cancellation);

        /// <summary>
        /// Добавляет заказ
        /// </summary>
        /// <param name="productId">Идентификатор продукта</param>
        /// <param name="quantity">Количество продукта</param>
        /// <param name="cancellation">Токен отмены операции</param>
        Task AddOrderAsync(CartDto cart, OrderDto orderDto, CancellationToken cancellation);


        /// <summary>
        /// Удаляет заказ
        /// </summary>
        /// <param name="productId">Идентификатор заказ</param>
        /// <param name="cancellation">Токен отмены операции</param>
        Task DeleteOrderAsync(int orderId, CancellationToken cancellation);
    }
}
