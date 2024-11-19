using AutoMapper;
using OnlineStore.AppServices.Common.DataTimeProviders;
using OnlineStore.AppServices.Common.Events.Common;
using OnlineStore.AppServices.Products.Repositories;
using OnlineStore.Contracts.Common;
using OnlineStore.Contracts.Product;
using OnlineStore.Domain.Entities;
using OnlineStore.Domain.Events;

namespace OnlineStore.AppServices.Products.Services
{
    public sealed class ProductsService : IProductsService
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        private readonly IEventAccumulator _eventContainer;
        private readonly IDataTimeProvider _dataTimeProvider;

        public ProductsService(IProductRepository repository,
            IMapper mapper,
            IEventAccumulator eventContainer,
             IDataTimeProvider dataTimeProvider)
        {
            _repository = repository;
            _mapper = mapper;
            _eventContainer = eventContainer;
            _dataTimeProvider = dataTimeProvider;
		}

        /// <inheritdoc/>
        public async Task AddProductAsync(ShortProductDto productDto,CancellationToken cancellation)
        {
            var domainProduct = _mapper.Map<Product>(productDto);

            _eventContainer.AddEvent(new AddProductEvent
            {
                eventDate = _dataTimeProvider.UtcNow,
                productName = "productName"
            });

            await _repository.AddAsync(domainProduct, cancellation);
        }



        /// <inheritdoc/>
        public Task<List<Product>> GetProductAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<ShortProductDto> GetProductByIdAsync(int productId, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<ProductsListDto> GetProductsAsync(PagedRequest request, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }
    }
}
