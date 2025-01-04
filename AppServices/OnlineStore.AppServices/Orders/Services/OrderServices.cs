using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using OnlineStore.AppServices.Carts.Repositories;
using OnlineStore.AppServices.Carts.Services;
using OnlineStore.AppServices.Common.DataTimeProviders;
using OnlineStore.AppServices.Common.Events.Common;
using OnlineStore.AppServices.Orders.Repositories;
using OnlineStore.AppServices.Products.Repositories;
using OnlineStore.AppServices.Products.Services;
using OnlineStore.Contracts.Carts;
using OnlineStore.Contracts.Common;
using OnlineStore.Contracts.Enums;
using OnlineStore.Contracts.Order;
using OnlineStore.Contracts.Product;
using OnlineStore.Domain.Entities;

namespace OnlineStore.AppServices.Orders.Services
{
    public sealed class OrderServices : IOrderServices
    {

        private readonly IOrderRepository _orderRepository;
        private readonly IEventAccumulator _eventContainer;
        private readonly IDataTimeProvider _dataTimeProvider;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProductsService _productService;


        public OrderServices(IDataTimeProvider dataTimeProvider,
            IEventAccumulator eventContainer,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IOrderRepository orderRepository,
            IProductsService productService)
        {
            _eventContainer = eventContainer;
            _dataTimeProvider = dataTimeProvider;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _orderRepository = orderRepository;
            _productService = productService;
        }


        public async Task AddOrderAsync(CartDto cart, OrderDto orderDto, CancellationToken cancellation)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);



            if (cart != null)
            {
                var existingOrder = new Order
                {
                    UserId = user.Id,
                    userName = orderDto.userName,
                    addressUser = orderDto.addressUser,
                    numberPhoneUser = orderDto.numberPhoneUser,
                    TotalAmount = cart.TotalAmount,
                    OrderDate = _dataTimeProvider.UtcNow,
                    OrderStatusId = (int)OrderStatusEnum.New,
                };

                AddOrderItemsToOrder(existingOrder, cart, orderDto);

                await _orderRepository.AddAsync(existingOrder, cancellation);
            }
            else
            {

            }
        }

        private static void AddOrderItemsToOrder(Order order, CartDto cart, OrderDto orderDto)
        {
            foreach (var item in cart.Items)
            {

                order.orderItems.Add(new OrderItem
                {
                    Order = order,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price * item.Quantity
                });
            }
        }


        public Task DeleteOrderAsync(int orderId, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public async Task<OrderDto> GetOrderAsync(CancellationToken cancellation)
        {
            var order = await GetCurrentUserOrderAsync(cancellation);

            if (order == null)
            {
                return new OrderDto
                {
                    orderItems = [],
                    TotalAmount = 0
                };
            }

            return await GetOrderItemsAsync(order, cancellation);
        }
        private async Task<List<Order>> GetCurrentUserOrderAsync(CancellationToken cancellation)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            if (user == null)
            {
                return null;
            }
            return await _orderRepository.GetOrderByUserAsync(user.Id, cancellation);

        }
        private async Task<OrderDto> GetOrderItemsAsync(List<Order> order, CancellationToken cancellation)
        {
            var products = await _productService.GetProductsAsync(new PagedRequest(), cancellation);

            var orderItem = new List<OrderItemDto>(products.Result.Count);

            var totalAmount = 0m;

            foreach (var items in order)
            {
                foreach (var item in items.orderItems)
                {
                    var product = products.Result.FirstOrDefault(x => x.Id == item.ProductId);
                    orderItem.Add(new OrderItemDto
                    {
                        UnitPrice = product.Price,
                        ProductId = product.Id,
                        ProductName = product.Name,
                        OrderId = item.OrderId,
                        Quantity = item.Quantity,
                    });

                    totalAmount += product.Price * item.Quantity;
                }
            }

            return new OrderDto
            {
                orderItems = orderItem,
                TotalAmount = Math.Round(totalAmount, 2)
            };
        }
    }
}
