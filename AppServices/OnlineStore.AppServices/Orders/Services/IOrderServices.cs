using OnlineStore.Contracts.Carts;
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
        Task<OrderDto> GetOrdersAllAsync(CancellationToken cancellation);

        Task<List<OrderDto>> GetOrderAsync( CancellationToken cancellation);

        Task<OrderDto> GetOrderIdAsync(int orderId, CancellationToken cancellation);

        /// <summary>
        /// Добавляет заказ
        /// </summary>
        /// <param name="productId">Идентификатор продукта</param>
        /// <param name="quantity">Количество продукта</param>
        /// <param name="cancellation">Токен отмены операции</param>
        Task AddOrderAsync(CartDto cart, OrderDto orderDto, CancellationToken cancellation);


        /// <summary>
        /// Отменяет заказ
        /// </summary>
        /// <param name="productId">Идентификатор заказ</param>
        /// <param name="cancellation">Токен отмены операции</param>
        Task CancelOrderProduct(int orderId, int productId, CancellationToken cancellation);

        Task CancelOrderAsync(int orderId, CancellationToken cancellation);

    }
}
