using OnlineStore.AppServices.Common.CacheServices;
using OnlineStore.Contracts.ProductAttributes;

namespace OnlineStore.AppServices.Attributes.Services
{
	/// <summary>
	/// Кэширующий декоратор для сервисов атрибутов продуктов
	/// </summary>
	public sealed class CachedProductAttributeService : IProductAttributeService
	{
		private readonly IProductAttributeService _productAttributeService;
		private readonly  ICacheService _cacheService;

		/// <inheritdoc/>
		public CachedProductAttributeService(IProductAttributeService productAttributeService,
			ICacheService cacheService)
        {
			_productAttributeService = productAttributeService;
			_cacheService = cacheService;
		}

		/// <inheritdoc/>
		public async Task<ProductAttributeDto> GetAsync(int id)
		{
			var attribute = await _cacheService.GetOrSetAsync(key: $"ProductAttributes_{id}",
				TimeSpan.FromMinutes(10),
				func: async () => (await _productAttributeService.GetAsync(id)),
				cancellation: CancellationToken.None);

			return attribute;
		}
	}
}
