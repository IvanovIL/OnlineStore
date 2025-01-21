using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using OnlineStore.AppServices.Common.DataTimeProviders;
using OnlineStore.AppServices.Common.Events.Common;
using OnlineStore.AppServices.Orders.Repositories;
using OnlineStore.AppServices.Products.Repositories;
using OnlineStore.AppServices.Products.Services;
using OnlineStore.Contracts.Carts;
using OnlineStore.Contracts.Common;
using OnlineStore.Contracts.Enums;
using OnlineStore.Contracts.Order;
using OnlineStore.Domain.Entities;


namespace OnlineStore.AppServices.Orders.Services
{
    public sealed class OrderServices : IOrderServices
    {

        private readonly IOrderRepository _orderRepository;
        private readonly IDataTimeProvider _dataTimeProvider;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProductsService _productService;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;


        public OrderServices(IDataTimeProvider dataTimeProvider,
            IEventAccumulator eventContainer,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IOrderRepository orderRepository,
            IProductsService productService,
            IMapper mapper,
             IProductRepository productRepository)
        {
            _productRepository = productRepository;
            _dataTimeProvider = dataTimeProvider;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _orderRepository = orderRepository;
            _productService = productService;
            _mapper = mapper;
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
                throw new Exception("Ошибка корзина пуста");
            }
        }

        private static void AddOrderItemsToOrder(Order order,CartDto cart, OrderDto orderDto)
        {
           


            foreach (var item in cart.Items)
            {

                order.orderItems.Add(new OrderItem
                {
                    Order = order,
                    IsDeleted = false,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    ProductName = item.ProductName,
                    UnitPrice = item.Price * item.Quantity

                });
            }


        }
        public async Task CancelOrderAsync(int orderId, CancellationToken cancellation)
        {
            var order = await _orderRepository.GetOrderAsync(orderId);

            foreach (var item in order.orderItems)
            {
                var product = await _productRepository.GetAsync(item.ProductId);
                product.stockQuantity += item.Quantity;

                await _productRepository.UpdateAsync(product, cancellation);

            }

            order.OrderStatusId = 6;

            await _orderRepository.UpdateAsync(order, cancellation);

        }




        public async Task CancelOrderProduct(int orderId, int productId, CancellationToken cancellation)
        {
            var orderItem = await _orderRepository.GetOrderItemAsync(orderId);

            int totalAmountDeleted = 0;

            foreach (var item in orderItem)
            {
                if (productId == item.ProductId)
                {
                    var product = await _productRepository.GetAsync(item.ProductId);
                    product.stockQuantity += item.Quantity;
                    await _productRepository.UpdateAsync(product, cancellation);
                    item.IsDeleted = true;
                    totalAmountDeleted++;
                }
                else if (item.IsDeleted == true)
                {
                    totalAmountDeleted++;
                }
            }
            

            if (totalAmountDeleted == 0)
            {
                foreach (var item in orderItem)
                {
                    if (item.OrderId == orderId)
                    {
                        await _orderRepository.updateOrderItem(item, cancellation);
                    }
                }

            }
            else if(totalAmountDeleted == orderItem.Count)
            {
                var order = await _orderRepository.GetOrderAsync(orderId);

                order.OrderStatusId = 6;

                await _orderRepository.UpdateAsync(order, cancellation);
            }


        }


        public async Task<OrderDto> GetOrdersAllAsync(CancellationToken cancellation)
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
            return await _orderRepository.GetOrdersAsync(user.Id);

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

        public async Task<List<OrderDto>> GetOrderAsync(CancellationToken cancellation)
        {
            var orders = await GetOrder(cancellation);

            if (orders == null)
            {
                return new List<OrderDto>();

            }


            var ordersList = _mapper.Map<List<OrderDto>>(orders);



            return ordersList;

        }

        private async Task<List<Order>> GetOrder(CancellationToken cancellation)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            if (user == null)
            {
                return null;
            }
            return await _orderRepository.GetOrdersAsync(user.Id);

        }


        public async Task<OrderDto> GetOrderIdAsync(int orderId, CancellationToken cancellation)
        {
            var products = await _orderRepository.GetOrderItemAsync(orderId);

            if (products == null)
            {
                return null;
            }

            List<OrderItemDto> productDto = new List<OrderItemDto>();

            productDto = _mapper.Map<List<OrderItemDto>>(products);


            List<OrderItemDto> orderItem = new List<OrderItemDto>();

            var totalAmount = 0m;
            for (int i = 0; i < productDto.Count; i++)
            {
                orderItem.Add(new OrderItemDto
                {
                    UnitPrice = productDto[i].UnitPrice,
                    ProductId = productDto[i].ProductId,
                    ProductName = productDto[i].ProductName,
                    OrderId = orderId,
                    Quantity = productDto[i].Quantity,
                });
                totalAmount += products[i].UnitPrice * productDto[i].Quantity;

            }


            return new OrderDto
            {
                orderItems = orderItem,
                TotalAmount = Math.Round(totalAmount, 2)
            };
        }


    }
}
