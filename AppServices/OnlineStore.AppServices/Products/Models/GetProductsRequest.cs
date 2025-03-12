

namespace OnlineStore.AppServices.Products.Models
{
	/// <summary>
	/// Получение элементов главной страницы
	/// </summary>
	public sealed class GetProductsRequest
	{
        public int Take { get; set; }

        public int Skip { get; set; }

		public bool IncludeCategory { get; set; }

		public bool IncludeImages { get; set; }
	}
}
