using AutoMapper;
using OnlineStore.Contracts.Product;
using OnlineStore.Domain.Entities;

namespace OnlineStore.Infrastructure.Mappings
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<ShortProductDto, Product>();
            CreateMap<Product, ShortProductDto>();
        }
    }
}
