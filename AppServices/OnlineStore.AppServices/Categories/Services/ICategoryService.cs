using OnlineStore.Contracts.Categories;
using OnlineStore.Contracts.Common;
using OnlineStore.Contracts.Product;


namespace OnlineStore.AppServices.Categories.Services
{
    /// <summary>
    /// Интерфейс работы с категориями продуктов
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// Получате категорию продуктов
        /// </summary>
        /// <param name="cancellation">Токен отмены операции</param>
        Task<IReadOnlyCollection<CategoryDto>> GetCategoriesAsync(CancellationToken cancellation);

        /// <summary>
        /// Добавляет новую категорию продуктов
        /// </summary>
        /// <param name="categoryDto">Категория</param>
        /// <param name="cancellation">Токен отмены операции</param>
        Task AddCategoryAsync(CategoryDto categoryDto, CancellationToken cancellation);

        /// <summary>
        /// Получает продукты категории по идентификатору
        /// </summary>
        /// <param name="CategoryId">Идентификатор</param>
        /// <param name="request">Страница с продуктами</param>
        /// <param name="cancellation">Токен отмены операции</param>
        Task<ProductsListDto> findCatregoryAsync(int CategoryId,PagedRequest request, CancellationToken cancellation);
    }
}
