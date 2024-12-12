using AutoMapper;
using OnlineStore.AppServices.Categories.Repositories;
using OnlineStore.AppServices.Common.DataTimeProviders;
using OnlineStore.AppServices.Common.Events.Common;
using OnlineStore.AppServices.Images.Services;
using OnlineStore.AppServices.Products.Models;
using OnlineStore.AppServices.Products.Repositories;
using OnlineStore.Contracts.Categories;
using OnlineStore.Contracts.Common;
using OnlineStore.Contracts.Product;
using OnlineStore.Domain.Entities;
using OnlineStore.Domain.Events;
using System.Net.WebSockets;

namespace OnlineStore.AppServices.Products.Services
{
    public sealed class ProductsService : IProductsService
    {
        private readonly IProductRepository _repository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IEventAccumulator _eventContainer;
        private readonly IDataTimeProvider _dataTimeProvider;
        private readonly IImageService _imageService;

        public ProductsService(IProductRepository repository,
            IMapper mapper,
            IEventAccumulator eventContainer,
             IDataTimeProvider dataTimeProvider,
             IImageService imageService)
        {
            _repository = repository;
            _mapper = mapper;
            _eventContainer = eventContainer;
            _dataTimeProvider = dataTimeProvider;
            _imageService = imageService;
        }

        /// <inheritdoc/>
        public async Task AddProductAsync(ShortProductDto productDto, CancellationToken cancellation)
        {
            var domainProduct = _mapper.Map<Product>(productDto);

            domainProduct.createdAt = _dataTimeProvider.UtcNow;

            _eventContainer.AddEvent(new AddProductEvent
            {
                eventDate = _dataTimeProvider.UtcNow,
                productName = domainProduct.Name,
            });

            await _repository.AddAsync(domainProduct, cancellation);
        }



        /// <inheritdoc/>
        public async Task<ShortProductDto> GetProductByIdAsync(int productId, CancellationToken cancellation)
        {
            var product = await _repository.GetAsync(productId) ?? throw new Exception($"Не найден продукт Id = {productId}");

            var result = _mapper.Map<ShortProductDto>(product);
            result.ImagesUrls = _imageService.GetImagesUrls(product.Images.ToArray());
            return result;
        }

        /// <inheritdoc/>
        public async Task<ProductsListDto> GetProductsAsync(PagedRequest request, CancellationToken cancellation)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var totalCount = await _repository.GetProductsTotalCountAsync(cancellation);

            if (totalCount == 0)
            {
                return new ProductsListDto
                {
                    PageNumber = 1,
                    TotalCount = totalCount,
                    PageSize = 1,
                    Result = []
                };
            }
            var products = await _repository.GetProductsAsync(new GetProductsRequest
            {
                Take = request.PageSize,
                Skip = (request.PageNumber - 1) * request.PageSize,
                IncludeCategory = true
            }, cancellation);

            var productList = _mapper.Map<List<ShortProductDto>>(products);

            return new ProductsListDto
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount,
                Result = productList
            };
        }

        public async Task DeleteProductAsync(string name , CancellationToken cancellation)
        {
            List<Product> productsList = await _repository.GetAllAsync(cancellation);
            int id = 0;
            foreach (var item in productsList)
            {
                if (item.Name == name)
                {
                    id = item.Id; 
                    break;
                }
            }

            var product = await _repository.GetAsync(id) ?? throw new Exception($"Не найден продукт Id = {id}");

            product.IsDeleted = true;

            await _repository.DeleteAsync(product, cancellation);

        }

        public async Task ChangeProductAsync(ShortProductDto productDto, CancellationToken cancellation)
        {

            var domainProduct = _mapper.Map<Product>(productDto);

            domainProduct.UpdatedAt = _dataTimeProvider.UtcNow;

            _eventContainer.AddEvent(new AddProductEvent
            {
                eventDate = _dataTimeProvider.UtcNow,
                productName = "productName"
            });

            await _repository.UpdateAsync(domainProduct, cancellation);

        }

        public async Task<ShortProductDto> FindProductAsync(string name, CancellationToken cancellation)
        {
            var productAll = await _repository.GetAllAsync(cancellation);

            int productId = 0;
            foreach (var item in productAll)
            {
                if(item.Name == name)
                {
                    productId = item.Id;
                    break;
                }
               
                
            }

            var product = await _repository.GetAsync(productId) ?? throw new Exception($"Не найден продукт Id = {productId}");

            var result = _mapper.Map<ShortProductDto>(product);
            result.ImagesUrls = _imageService.GetImagesUrls(product.Images.ToArray());
            return result;

        }
    }
}
