
using OnlineStore.Contracts.Categories;

namespace OnlineStore.AppServices.Categories.Services
{
    public interface ICategoryService
    {
        Task<IReadOnlyCollection<CategoryDto>> GetCategoriesAsync(CancellationToken cancellation);
    }
}
