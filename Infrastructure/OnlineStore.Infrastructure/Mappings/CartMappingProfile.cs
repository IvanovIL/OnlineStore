using AutoMapper;
using OnlineStore.Contracts.Carts;
using OnlineStore.Contracts.Product;


namespace OnlineStore.Infrastructure.Mappings
{
    public class CartMappingProfile : Profile
    {
        public CartMappingProfile()
        {
            CreateMap<ShortProductDto,CartItemDto>();
            CreateMap<CartItemDto, ShortProductDto>();
        }
    }
}
