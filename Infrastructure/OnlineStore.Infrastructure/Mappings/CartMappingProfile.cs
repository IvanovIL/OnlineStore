using AutoMapper;
using OnlineStore.Contracts.Carts;
using OnlineStore.Contracts.Product;
using OnlineStore.Domain.Entities;


namespace OnlineStore.Infrastructure.Mappings
{
    public class CartMappingProfile : Profile
    {
        public CartMappingProfile()
        {
            CreateMap<CartProduct, CartItemDto>();
            CreateMap<CartItemDto, CartProduct>();
            CreateMap<CartDto, Cart>();
            CreateMap<Cart, CartDto>();
            CreateMap<ShortProductDto,CartItemDto>();
            CreateMap<CartItemDto, ShortProductDto>();
        }
    }
}
