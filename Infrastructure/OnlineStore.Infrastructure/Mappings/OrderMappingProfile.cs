using AutoMapper;
using OnlineStore.Contracts.Order;
using OnlineStore.Domain.Entities;


namespace OnlineStore.Infrastructure.Mappings
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<Order, OrderDto>();
            CreateMap<OrderDto, Order>();
            CreateMap<OrderItemDto, OrderItem>();
            CreateMap<OrderItem, OrderItemDto>();
        }
    }
}
