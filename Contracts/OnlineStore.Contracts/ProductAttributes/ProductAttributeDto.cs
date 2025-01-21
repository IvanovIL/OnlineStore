

namespace OnlineStore.Contracts.ProductAttributes
{
	/// <summary>
	/// Транспаортная модель аттрибута продукта
	/// </summary>
	public sealed class ProductAttributeDto
	{
		/// <summary>
		/// Идентификатор 
		/// </summary>
		public int Id { get; set; }
		/// <summary>
		/// Наименование
		/// </summary>
		public string FullAttributeName { get; set; }
	}
}
