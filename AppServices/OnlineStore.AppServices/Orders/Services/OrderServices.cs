using AutoMapper;
using OnlineStore.AppServices.Common.DataTimeProviders;
using OnlineStore.AppServices.Common.Events.Common;
using OnlineStore.AppServices.Products.Repositories;
using OnlineStore.Contracts.Common;
using OnlineStore.Contracts.Order;
using OnlineStore.Contracts.Product;
using OnlineStore.Domain.Entities;
using OnlineStore.Domain.Events;

namespace OnlineStore.AppServices.Orders.Services
{
    public sealed class OrderServices : IOrderServices
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _repository;
        private readonly IEventAccumulator _eventContainer;
        private readonly IDataTimeProvider _dataTimeProvider;

        public OrderServices(IMapper mapper,
            IProductRepository repository, IDataTimeProvider dataTimeProvider,
            IEventAccumulator eventContainer)
        {
            _mapper = mapper;
            _repository = repository;
            _eventContainer = eventContainer;
            _dataTimeProvider = dataTimeProvider;
        }

        public async Task AddOrderAsync(OrderDto orderDto, CancellationToken cancellation)
        {
            var domainProduct = _mapper.Map<Order>(orderDto);

            domainProduct.OrderDate = _dataTimeProvider.UtcNow;

            //_eventContainer.AddEvent(new AddProductEvent
            //{
            //    eventDate = _dataTimeProvider.UtcNow,
            //    productName = "orderName"
            //});

            //await _repository.AddAsync(domainProduct, cancellation);

        }

        public Task DeleteOrderAsync(int orderId, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public Task<ProductsListDto> GetOrderAsync(PagedRequest request, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }
    }
}
