using OnlineStore.Contracts.Carts;
using OnlineStore.Contracts.Order;
using OnlineStore.Domain.Entities;

namespace OnlineStore.AppServices.Orders.Services
{
    /// <summary>
    /// Интерфейс по работе с заказами
    /// </summary>
    public interface IOrderServices 
    {
        /// <summary>
        /// Получает все заказы
        /// </summary>
        /// <param name="cancellation">Токен отмены операции</param>
        Task<OrderDto> GetOrdersAllAsync(CancellationToken cancellation);

        /// <summary>
        /// Получает список заказов
        /// </summary>
        /// <param name="cancellation">Токен отмены операции</param>
        Task<List<OrderDto>> GetOrderAsync(CancellationToken cancellation);

        /// <summary>
        /// Получает заказ по идентификатору
        /// </summary>
        /// <param name="orderId">Идентификатор заказа</param>
        /// <param name="cancellation">Токен отмены операции</param>
        Task<OrderDto> GetOrderIdAsync(int orderId, CancellationToken cancellation);

        /// <summary>
        /// Добавляет всю корзину продуктов заказ
        /// </summary>
        /// <param name="cart">Корзина с товарами</param>
        /// <param name="orderDto">Заказ</param>
        /// <param name="cancellation">Токен отмены операции</param>
        Task AddOrderAllItemsAsync(CartDto cart, OrderDto orderDto, CancellationToken cancellation);

        /// <summary>
        /// Добавляет один продукт из корзины в заказ
        /// </summary>
        /// <param name="cartItemDto">продукт из корзины</param>
        /// <param name="orderDto">Заказ</param>
        /// <param name="cancellation">Токен отмены операции</param>
        Task AddOrderAsync(CartItemDto cartItemDto, OrderDto orderDto, CancellationToken cancellation);


        /// <summary>
        /// Удаляет продукт из заказа и возращает его в БД
        /// </summary>
        /// <param name="productId">Идентификатор продукат</param>
        /// <param name="cancellation">Токен отмены операции</param>
        Task CancelOrderProduct(int orderId, int productId, CancellationToken cancellation);

        /// <summary>
        /// Отменяет заказ
        /// </summary>
        /// <param name="orderId">Идентификатор заказ</param>
        /// <param name="cancellation">Токен отмены операции</param>
        Task CancelOrderAsync(int orderId, CancellationToken cancellation);

        /// <summary>
        /// Создает IEnumerable для управления статусом заказа
        /// </summary>
        Task<IEnumerable<OrderStatus>> OrderStatusDto();

        /// <summary>
        /// Измеяет статус заказа
        /// </summary>
        /// <param name="id">Идентификатор заказа</param>
        /// <param name="statusOrder">Идентификатор статуса заказа</param>
        /// <param name="cancellation">Токен отмены  операции</param>
        Task changeStatusOrder(int id, int statusOrder, CancellationToken cancellation);
    }
}
