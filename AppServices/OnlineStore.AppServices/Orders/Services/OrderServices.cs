using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using OnlineStore.AppServices.Carts.Repositories;
using OnlineStore.AppServices.Common.DataTimeProviders;
using OnlineStore.AppServices.Common.Events.Common;
using OnlineStore.AppServices.Orders.Repositories;
using OnlineStore.AppServices.Products.Repositories;
using OnlineStore.AppServices.Products.Services;
using OnlineStore.Contracts.Carts;
using OnlineStore.Contracts.Enums;
using OnlineStore.Contracts.Order;
using OnlineStore.Domain.Entities;
using OnlineStore.Domain.Events;
using OnlineStore.Infrastructure.Extensions;

namespace OnlineStore.AppServices.Orders.Services
{
    /// <summary>
    /// Сервис по работе с заказами
    /// </summary>
    public sealed class OrderServices : IOrderServices
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IDataTimeProvider _dataTimeProvider;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProductsService _productService;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IEventAccumulator _eventContainer;

        public OrderServices(IDataTimeProvider dataTimeProvider,
            IEventAccumulator eventContainer,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IOrderRepository orderRepository,
            ICartRepository cartRepository,
            IProductsService productService,
            IMapper mapper,
             IProductRepository productRepository)
        {
            _productRepository = productRepository;
            _dataTimeProvider = dataTimeProvider;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _productService = productService;
            _mapper = mapper;
            _eventContainer = eventContainer;
        }

        /// <inheritdoc/>
        public async Task AddOrderAllItemsAsync(CartDto cart, OrderDto orderDto, CancellationToken cancellation)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User) ??
                throw new InvalidOperationException();

            if (cart != null)
            {
                var existingOrder = new Order
                {
                    UserId = user.Id,
                    userName = orderDto.userName,
                    addressUser = orderDto.addressUser,
                    numberPhoneUser = orderDto.numberPhoneUser,
                    TotalAmount = cart.TotalAmount,
                    OrderDate = _dataTimeProvider.Now,
                    OrderStatusId = (int)OrderStatusEnum.New,
                };

                AddOrderItemsToOrder(existingOrder, cart);

                await _orderRepository.AddAsync(existingOrder, cancellation);

                string productsName = string.Empty;

                for (int i = 0; i < cart.Items.Count; i++)
                {
                    if (productsName != string.Empty)
                    {

                        productsName += ", " + cart.Items[i].ProductName;
                    }
                    else
                    {
                        productsName = cart.Items[i].ProductName;
                    }

                }
                _eventContainer.AddEvent(new AddOrderProductsEvent
                {
                    eventDate = _dataTimeProvider.Now,
                    productName = "Ваш заказ продукты - " + productsName + " успешно создан",
                    Email = user.Email
                });
            }
            else
            {
                throw new Exception("Ошибка корзина пуста");
            }
        }

        /// <summary>
        /// Добавляет товар в заказ
        /// </summary>
        /// <param name="order">Заказ</param>
        /// <param name="cart">Коризина</param>
        private static void AddOrderItemsToOrder(Order order, CartDto cart)
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

        /// <inheritdoc/>
        public async Task AddOrderAsync(CartItemDto cartItemDto, OrderDto orderDto, CancellationToken cancellation)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User) ??
                throw new NullReferenceException();
            var cart = _mapper.Map<CartDto>(await _cartRepository.GetCartByUserAsync(user.Id, cancellation)
                ?? throw new Exception("Корзина не найдена"));

            if (cart != null)
            {
                var existingOrder = new Order
                {
                    UserId = user.Id,
                    userName = orderDto.userName,
                    addressUser = orderDto.addressUser,
                    numberPhoneUser = orderDto.numberPhoneUser,
                    TotalAmount = cartItemDto.Price * cartItemDto.Quantity,
                    OrderDate = _dataTimeProvider.Now,
                    OrderStatusId = (int)OrderStatusEnum.New,

                };

                existingOrder.orderItems.Add(new OrderItem
                {
                    Order = existingOrder,
                    IsDeleted = false,
                    ProductId = cartItemDto.ProductId,
                    Quantity = cartItemDto.Quantity,
                    ProductName = cartItemDto.ProductName,
                    UnitPrice = cartItemDto.Price * cartItemDto.Quantity
                });

                _eventContainer.AddEvent(new AddOrderProductsEvent
                {
                    eventDate = _dataTimeProvider.Now,
                    productName = "Ваш заказ продукт - " + cartItemDto.ProductName + " успешно создан",
                    Email = user.Email
                });

                await _orderRepository.AddAsync(existingOrder, cancellation);
            }
        }

        /// <inheritdoc/>
        public async Task CancelOrderAsync(int orderId, CancellationToken cancellation)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User)
                ?? throw new NullReferenceException();

            var order = await _orderRepository.GetOrderAsync(orderId);

            var productsName = "";

            foreach (var item in order.orderItems)
            {
                if(productsName == string.Empty)
                {
                    productsName = item.ProductName;
                }
                else
                {
                    productsName += ", " + item.ProductName;
                }

                var product = await _productRepository.GetAsync(item.ProductId);
                product.stockQuantity += item.Quantity;

                await _productRepository.UpdateAsync(product, cancellation);
            }

            order.OrderStatusId = (int)OrderStatusEnum.Canceled;

            _eventContainer.AddEvent(new AddOrderProductsEvent
            {
                eventDate = _dataTimeProvider.Now,
                Email = user.Email,
                productName = "Ваш заказа на продукты - " + productsName + "успешно отменен"

            });

            await _orderRepository.UpdateAsync(order, cancellation);
        }

        /// <inheritdoc/>
        public async Task CancelOrderProduct(int orderId, int productId, CancellationToken cancellation)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User) 
                ?? throw new NullReferenceException();
            var orderItem = await _orderRepository.GetOrderItemAsync(orderId);

            var product = await _productRepository.GetAsync(productId);

            int totalAmountDeleted = 0;

            decimal total = 0;

            foreach (var item in orderItem)
            {
                if (productId == item.ProductId)
                {
                    product.stockQuantity += item.Quantity;
                    total = item.UnitPrice;
                    await _productRepository.UpdateAsync(product, cancellation);
                    item.IsDeleted = true;
                    totalAmountDeleted++;
                    await _orderRepository.updateOrderItem(item, cancellation);
                }
                else if (item.IsDeleted == true)
                {
                    totalAmountDeleted++;
                }
            }
            _eventContainer.AddEvent(new AddOrderProductsEvent
            {
                eventDate = DateTime.Now,
                Email = user.Email,
                productName = "Заказ на продукт - " + product.Name + " отменен",
            });
            var order = await _orderRepository.GetOrderAsync(orderId);

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
            else if (totalAmountDeleted == orderItem.Count)
            {
                order.OrderStatusId = (int)OrderStatusEnum.Canceled;

                UpdateOrder(order, cancellation);
            }
            else
            {
                order.TotalAmount -= total;

                UpdateOrder(order, cancellation);
            }

        }

        /// <summary>
        /// Обновляет заказ
        /// </summary>
        /// <param name="order">Заказ</param>
        /// <param name="cancellation">Токен отмены операции</param>
        public async Task UpdateOrder(Order order, CancellationToken cancellation)
        {
            await _orderRepository.UpdateAsync(order, cancellation);
        }

        /// <inheritdoc/>
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

        /// <summary>
        /// Получает заказа текущего пользователя
        /// </summary>
        /// <param name="cancellation">Токен отмены операции</param>
        private async Task<List<Order>> GetCurrentUserOrderAsync(CancellationToken cancellation)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User)
                ?? throw new NullReferenceException();

            return await _orderRepository.GetOrdersAsync(user.Id);
        }

        /// <summary>
        /// Получает товар и помещяет его в заказ
        /// </summary>
        /// <param name="order">Заказ</param>
        /// <param name="cancellation">Токен отмены операции</param>
        private async Task<OrderDto> GetOrderItemsAsync(List<Order> order, CancellationToken cancellation)
        {
            var orderItem = new List<OrderItemDto>(order.Count);

            var totalAmount = 0m;

            foreach (var items in order)
            {
                foreach (var item in items.orderItems)
                {
                    var product = items.orderItems.FirstOrDefault(x => x.ProductId == item.ProductId)
                        ?? throw new NullReferenceException();

                    orderItem.Add(new OrderItemDto
                    {
                        UnitPrice = product.UnitPrice,
                        ProductId = product.Id,
                        ProductName = product.ProductName,
                        OrderId = item.OrderId,
                        Quantity = item.Quantity,

                    });

                    totalAmount += product.UnitPrice;
                }
            }

            return new OrderDto
            {
                orderItems = orderItem,
                TotalAmount = Math.Round(totalAmount, 2)
            };
        }

        /// <inheritdoc/>
        public async Task<List<OrderDto>> GetOrderAsync(CancellationToken cancellation)
        {
            var orders = await GetOrder(cancellation);

            if (orders == null)
            {
                return new List<OrderDto>();
            }

            var ordersList = _mapper.Map<List<OrderDto>>(orders);

            foreach (var order in ordersList)
            {
                order.OrderStatusDto = await EnumOrder(order.OrderStatusDto, order.OrderStatusId);
            }

            return ordersList;
        }


        /// <summary>
        /// Получает заказ текущего пользователя
        /// </summary>
        /// <param name="cancellation">Токен отмены операции</param>
        private async Task<List<Order>> GetOrder(CancellationToken cancellation)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User)
                ?? throw new NullReferenceException();

            return await _orderRepository.GetOrdersAsync(user.Id) ??
                throw new NullReferenceException();
        }

        /// <inheritdoc/>
        public async Task<OrderDto> GetOrderIdAsync(int orderId, CancellationToken cancellation)
        {
            var orders = await _orderRepository.GetOrderAsync(orderId)
                ?? throw new NullReferenceException(nameof(orderId));

            OrderDto orderDto = _mapper.Map<OrderDto>(orders);

            orderDto.OrderStatusDto = await EnumOrder(orderDto.OrderStatusDto, orderDto.OrderStatusId);

            return orderDto;
        }

        /// <inheritdoc/>
        public async Task changeStatusOrder(int id, int statusOrder, CancellationToken cancellation)
        {
            var order = await _orderRepository.GetAsync(id) ??
                throw new NullReferenceException(nameof(id));
            order.OrderStatusId = statusOrder;

            await UpdateOrder(order, cancellation);
        }


        /// <inheritdoc/>
        public async Task<IEnumerable<OrderStatus>> OrderStatusDto()
        {
            IEnumerable<OrderStatus> enumerable = Enum.GetValues(typeof(OrderStatusEnum))
                           .Cast<OrderStatusEnum>()
                           .Select(e => new OrderStatus()
                           {
                               Id = (int)e,
                               Name = e.GetEnumDescription()
                           });

            return enumerable;
        }

        /// <summary>
        /// Присваивает нужный статус по идентификатору
        /// </summary>
        /// <param name="orderStatusDto">Статус заказа</param>
        /// <param name="statusId">Идентификатор статуса заказа</param>
        private async Task<OrderStatusDto> EnumOrder(OrderStatusDto orderStatusDto, int statusId)
        {
            IEnumerable<OrderStatus> enumerable = Enum.GetValues(typeof(OrderStatusEnum))
                           .Cast<OrderStatusEnum>()
                           .Select(e => new OrderStatus()
                           {
                               Id = (int)e,
                               Name = e.GetEnumDescription()
                           });

            orderStatusDto = _mapper.Map<OrderStatusDto>(enumerable.ElementAtOrDefault(statusId - 1));

            return orderStatusDto;
        }
    }
}
