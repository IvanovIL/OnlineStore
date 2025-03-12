using OnlineStore.AppServices.Common;
using OnlineStore.AppServices.Products.Models;
using OnlineStore.Domain.Entities;

namespace OnlineStore.AppServices.Products.Repositories
{
	public interface IProductRepository : IRepository<Product>
	{
        /// <summary>
        /// Получает список продуктов
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellation">Токен отмены операции</param>
		Task<List<Product>> GetProductsAsync(GetProductsRequest request,CancellationToken cancellation);

        /// <summary>
        /// Пролучает количество продуктов
        /// </summary>
        /// <param name="cancellation">Токен отмены операции</param>
        Task<int> GetProductsTotalCountAsync(CancellationToken cancellation);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <param name="request"></param>
        /// <param name="cancellation"></param>
        Task<List<Product>> GetCategoryAsync(int CategoryId, GetProductsRequest request, CancellationToken cancellation);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <param name="cancellation"></param>
        Task<int> GetCategoryTotalCountAsync(int CategoryId, CancellationToken cancellation);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="Price"></param>
        /// <param name="request"></param>
        /// <param name="cancellation"></param>
        Task<List<Product>> GetProducts(string name, decimal Price, GetProductsRequest request, CancellationToken cancellation);
    }
}
