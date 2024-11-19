

using OnlineStore.AppServices.Common;
using OnlineStore.AppServices.Products.Models;
using OnlineStore.Domain.Entities;

namespace OnlineStore.AppServices.Products.Repositories
{
	public interface IProductRepository : IRepository<Product>
	{
		Task<List<Product>> GetProductsAsync(GetProductsRequest request,CancellationToken cancellation);
	}
}
