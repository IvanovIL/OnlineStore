using AutoMapper;
using OnlineStore.Contracts.Product;
using OnlineStore.Domain.Entities;

namespace OnlineStore.Infrastructure.Mappings
{
    public sealed class ShortProductDtoMappingProfile : Profile
    {
        public ShortProductDtoMappingProfile()
        {
            CreateMap<Product, ShortProductDto>();

        }


    }
}
