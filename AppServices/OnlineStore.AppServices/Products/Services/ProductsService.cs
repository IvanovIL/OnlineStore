using OnlineStore.AppServices.Products.Repositories;
using OnlineStore.Domain.Entities;

namespace OnlineStore.AppServices.Products.Services
{
    public sealed class ProductsService : IProductsService
    {
        private readonly IProductRepository _repository;
        public ProductsService(IProductRepository repository)
        {
            _repository = repository;

		}

        public Task AddProductAsync(CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public  Task<List<Product>> GetProductAsync()
        {


           return _repository.GetProductsAsync(new Models.GetProductsRequest
            {
                IncludeCategory = true
            });

        }

      
    }
}
