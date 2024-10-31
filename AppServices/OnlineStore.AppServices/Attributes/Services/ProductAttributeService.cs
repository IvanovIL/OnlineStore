
using AutoMapper;
using OnlineStore.AppServices.Attributes.Repositories;
using OnlineStore.Contracts.ProductAttributes;

namespace OnlineStore.AppServices.Attributes.Services
{
	public sealed class ProductAttributeService : IProductAttributeService
	{
		private readonly IAttributeRepository _attributeRepository;
		private readonly IMapper _mapper;

		/// <inheritdoc/>
		public ProductAttributeService(IAttributeRepository attributeRepository,
			IMapper mapper)
        {
			_attributeRepository = attributeRepository;
			_mapper = mapper;
		}
		
		/// <inheritdoc/>
        public async Task<ProductAttributeDto> GetAsync(int id)
		{
			var attribute = await _attributeRepository.GetAsync(id)
				?? throw new Exception($"Не найден атрибут с id = {id}");

			return _mapper.Map<ProductAttributeDto>(attribute);

			
			
		}
	}
}
