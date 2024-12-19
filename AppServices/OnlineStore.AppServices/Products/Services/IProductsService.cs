using OnlineStore.Contracts.Categories;
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
        Task DeleteProductAsync(string name, CancellationToken cancellation);

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
        Task<ProductsListDto> FindProductAsync(string nameProduct, PagedRequest request, CancellationToken cancellation);

    }
}
