using OnlineStore.Contracts.Carts;
using OnlineStore.Contracts.Common;
using OnlineStore.Contracts.Product;

namespace OnlineStore.AppServices.Products.Services
{
    public interface IProductsService
    {


        /// <summary>
        /// Возвращает список товаров
        /// </summary>
        /// <param name="request">Запрос на получение списка товаров</param>
        /// <param name="cancellation">Токен отмены операции</param>
        Task<ProductsListDto> GetProductsAsync(PagedRequest request, CancellationToken cancellation);

        /// <summary>
        /// Добавляет товар
        /// </summary>
        /// <param name="productDto">Транспортная модель товара</param>
        /// <param name="cancellation">Токен отмены операции</param>
        Task AddProductAsync(ShortProductDto productDto, CancellationToken cancellation);

        /// <summary>
        /// Возвращает информацию о продукте по его идентификатору
        /// </summary>
        /// <param name="productId">Идентификатор продукта</param>
        /// <param name="cancellation">Токен отмены операции</param>
        Task<ShortProductDto> GetProductByIdAsync(int productId, CancellationToken cancellation);

        /// <summary>
        /// Удаляет продук
        /// </summary>
        /// <param name="name">Идентификатор продукта</param>
        /// <param name="cancellation">Токен отмены операции</param>
        Task DeleteProductAsync(int idIsDeleted, CancellationToken cancellation);

        /// <summary>
        /// Изменяет информацию продукта
        /// </summary>
        /// <param name="productDto">Транспортная модель товара</param>
        /// <param name="cancellation">Токен отмены операции</param>
        Task ChangeProductAsync(ShortProductDto productDto, CancellationToken cancellation);


        /// <summary>
        ///  Находит продукт по наименованию
        /// </summary>
        /// <param name="nameProduct">Наименование продукта</param>
        /// <param name="cancellation">Токен отмены операции</param>
        Task<ProductsListDto> FindProductAsync(ShortProductDto shortProductDto, PagedRequest request, CancellationToken cancellation);

        /// <summary>
        /// Возвращает количество из корзины товара обратно в БД при оплате
        /// </summary>
        /// <param name="productDto">Наименование продукта</param>
        /// <param name="cancellation">Токен отмены операции</param>
        Task CheckoutAsync(CartDto cart, CancellationToken cancellation);

        /// <summary>
        /// Возвращает количество из корзины товара обратно в БД при оплате
        /// </summary>
        /// <param name="cartItem">Товары в корзине</param>
        /// <param name="cancellation">Токен отмены операции</param>
        Task CheckoutItemAsync(CartItemDto cartItem, CancellationToken cancellation);

    }
}
