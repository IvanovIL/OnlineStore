
namespace OnlineStore.Domain.Entities
{
	/// <summary>
	/// Атрибут категории
	/// </summary>
	public sealed class ProductAttribute
	{
		/// <summary>
		/// Идентификатор 
		/// </summary>
		public int Id { get; set; }
		/// <summary>
		/// Наименование категории
		/// </summary>
		public string Name { get; set; }
	}
}
