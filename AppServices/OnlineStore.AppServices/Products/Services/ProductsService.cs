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

        public async Task<List<Product>> GetProductAsync()
        {
            var product = await _repository.GetAsync(1);

            var productCategory = product.Category;

            var parent = productCategory.ParentCategory;

            var result = await _repository.GetProductsAsync(new Models.GetProductsRequest
            {
                IncludeCategory = true
            });

            return result;
        }
    }
}
