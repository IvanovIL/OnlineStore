using AutoMapper;
using OnlineStore.AppServices.Attributes.Repositories;
using OnlineStore.AppServices.Common.CacheServices;
using OnlineStore.Contracts.ProductAttributes;

namespace OnlineStore.AppServices.Attributes.Services
{
	public sealed class ProductAttributeService : IProductAttributeService
	{
		private readonly IAttributeRepository _attributeRepository;
		private readonly IMapper _mapper;
		private readonly ICacheService _cacheService;

		/// <inheritdoc/>
		public ProductAttributeService(IAttributeRepository attributeRepository,
			IMapper mapper, ICacheService cacheService)
        {
			_attributeRepository = attributeRepository;
			_mapper = mapper;
			_cacheService = cacheService;
		}
		
		/// <inheritdoc/>
        public async Task<ProductAttributeDto> GetAsync(int id)
		{
			var attribute = await _attributeRepository.GetAsync(id);

			var dto = _mapper.Map<ProductAttributeDto>(attribute);

			return dto;
		}
	}
	
}
