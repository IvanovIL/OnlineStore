
using OnlineStore.Contracts.Categories;
using OnlineStore.Contracts.Common;
using OnlineStore.Contracts.Product;
using OnlineStore.Domain.Entities;
using System.ComponentModel;

namespace OnlineStore.AppServices.Categories.Services
{
    public interface ICategoryService
    {
        Task<IReadOnlyCollection<CategoryDto>> GetCategoriesAsync(CancellationToken cancellation);

        Task AddCategoryAsync(CategoryDto categoryDto, CancellationToken cancellation);

        Task<ProductsListDto> findCatregoryAsync(int CategoryId,PagedRequest request, CancellationToken cancellation);
    }
}
