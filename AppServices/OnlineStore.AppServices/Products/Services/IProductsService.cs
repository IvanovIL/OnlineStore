using OnlineStore.Domain.Entities;

namespace OnlineStore.AppServices.Products.Services
{
    public interface IProductsService
    {
        Task<List<Product>> GetProductAsync();

        Task AddProductAsync(CancellationToken cancellation);

    }
}
