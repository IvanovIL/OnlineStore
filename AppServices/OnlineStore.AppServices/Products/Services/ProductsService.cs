using AutoMapper;
using OnlineStore.AppServices.Common.DataTimeProviders;
using OnlineStore.AppServices.Common.Events.Common;
using OnlineStore.AppServices.Images.Services;
using OnlineStore.AppServices.Products.Models;
using OnlineStore.AppServices.Products.Repositories;
using OnlineStore.Contracts.Carts;
using OnlineStore.Contracts.Common;
using OnlineStore.Contracts.Product;
using OnlineStore.Domain.Entities;

namespace OnlineStore.AppServices.Products.Services
{
    /// <summary>
    /// Сервис для  работы с продуктом
    /// </summary>
    public sealed class ProductsService : IProductsService
    {
        private readonly IProductRepository _repository;
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
            domainProduct.createdAt = _dataTimeProvider.Now;
            domainProduct.Images = await _imageService.SaveProductImagesAsync(productDto.ImagesUrls, domainProduct, cancellation);

            await _repository.AddAsync(domainProduct, cancellation);
        }

        /// <inheritdoc/>
        public async Task CheckoutAsync(CartDto carts, CancellationToken cancellation)
        {
            foreach (var product in carts.Items)
            {
                var result = await _repository.GetAsync(product.ProductId);
                result.stockQuantity -= product.Quantity;
                await _repository.UpdateAsync(result, cancellation);
            }
        }

        /// <inheritdoc/>
        public async Task CheckoutItemAsync(CartItemDto cartItem, CancellationToken cancellation)
        {
            var result = await _repository.GetAsync(cartItem.ProductId);
            result.stockQuantity -= cartItem.Quantity;
            await _repository.UpdateAsync(result, cancellation);
        }


        /// <inheritdoc/>
        public async Task<ShortProductDto> GetProductByIdAsync(int productId, CancellationToken cancellation)
        {
            var product = await _repository.GetAsync(productId)
                ?? throw new Exception($"Не найден продукт Id = {productId}");

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


        /// <inheritdoc/>
        public async Task DeleteProductAsync(int id, CancellationToken cancellation)
        {
            var product = await _repository.GetAsync(id);

            product.IsDeleted = true;

            await _repository.UpdateAsync(product, cancellation);
        }

        /// <inheritdoc/>
        public async Task ChangeProductAsync(ShortProductDto productDto, CancellationToken cancellation)
        {
            var domainProduct = _mapper.Map<Product>(productDto);

            domainProduct.UpdatedAt = _dataTimeProvider.Now;

            await _repository.UpdateAsync(domainProduct, cancellation);
        }

        /// <inheritdoc/>
        public async Task<ProductsListDto> FindProductAsync(ShortProductDto productDto, PagedRequest request, CancellationToken cancellation)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var products = await _repository.GetProducts(productDto.Name, productDto.Price ,new GetProductsRequest
            {
                Take = request.PageSize,
                Skip = (request.PageNumber - 1) * request.PageSize,
                IncludeCategory = true
            }, cancellation);

            if(products.Count == 0)
            {
                return new ProductsListDto
                {
                    PageNumber = 1,
                    TotalCount = products.Count,
                    PageSize = 1,
                    Result = []
                };
            }

            var productList = _mapper.Map<List<ShortProductDto>>(products);

            return new ProductsListDto
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = products.Count,
                Result = productList
            };
        }
    }
}
