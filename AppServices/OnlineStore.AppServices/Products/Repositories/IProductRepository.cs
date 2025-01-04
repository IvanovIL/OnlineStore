

using OnlineStore.AppServices.Common;
using OnlineStore.AppServices.Products.Models;
using OnlineStore.Contracts.Product;
using OnlineStore.Domain.Entities;

namespace OnlineStore.AppServices.Products.Repositories
{
	public interface IProductRepository : IRepository<Product>
	{
		Task<List<Product>> GetProductsAsync(GetProductsRequest request,CancellationToken cancellation);


        Task<int> GetProductsTotalCountAsync(CancellationToken cancellation);

		Task<List<Product>> FindAsync(string name, GetProductsRequest request, CancellationToken cancellation);

        Task<int> GetProductsNameTotalCountAsync(string name, CancellationToken cancellation);

        Task<List<Product>> findCategoryAsync(int CategoryId, GetProductsRequest request, CancellationToken cancellation);

        Task<int> GetCategoryTotalCountAsync(int CategoryId, CancellationToken cancellation);
    }
}
