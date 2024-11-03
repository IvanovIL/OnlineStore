using AutoMapper;
using OnlineStore.AppServices.Attributes.Repositories;
using OnlineStore.AppServices.Common.Redis;
using OnlineStore.Contracts.ProductAttributes;

namespace OnlineStore.AppServices.Attributes.Services
{
	public sealed class ProductAttributeService : IProductAttributeService
	{
		private readonly IAttributeRepository _attributeRepository;
		private readonly IMapper _mapper;
		private readonly IRedisCache _redisCache;

		/// <inheritdoc/>
		public ProductAttributeService(IAttributeRepository attributeRepository,
			IMapper mapper, IRedisCache redisCache)
        {
			_attributeRepository = attributeRepository;
			_mapper = mapper;
			_redisCache = redisCache;
		}
		
		/// <inheritdoc/>
        public async Task<ProductAttributeDto> GetAsync(int id)
		{
			//var cachedValue = await _redisCache.GetAsync($"ProductAttributes_{id}");
			//if (cachedValue != null)
			//{
			//	return new ProductAttributeDto
			//	{
			//		Id = id,
			//		FullAttributeName = cachedValue
			//	};
			//}
			var attribute = await _attributeRepository.GetAsync(id)
				?? throw new Exception($"Не найден атрибут с id = {id}");

			var dto = _mapper.Map<ProductAttributeDto>(attribute);
			//await _redisCache.SetStringAsync($"ProductAttributes_{dto.Id} , {dto.FullAttributeName}");
			return dto;



		}
	}
}
