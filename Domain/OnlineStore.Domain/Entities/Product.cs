using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.Domain.Entities
{
	/// <summary>
	/// Товар
	/// </summary>
	public sealed class Product
	{
		/// <summary>
		/// Идентификатор продукта
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Наименование
		/// </summary>
		public string Name { get; set; } = default!;

		/// <summary>
		/// Описание продукта
		/// </summary>
		public string Description { get; set; } = default!;

		/// <summary>
		/// Цена продукта
		/// </summary>
		 [Column(TypeName = "decimal(18, 2)")]
		public decimal Price { get; set; }

		/// <summary>
		/// Ссылка на главное изображение товара
		/// </summary>
		public string? ImageUrl { get; set; }

		/// <summary>
		/// Количество товара
		/// </summary>
		public int stockQuantity { get; set; }

		/// <summary>
		/// Дата создания
		/// </summary>
		public DateTime createdAt { get; set; }

		/// <summary>
		/// Дата модификации
		/// </summary>
		public DateTime? updatedAt { get; set; }

		/// <summary>
		/// Признак удаление товара
		/// </summary>
		public bool isDeleted { get; set; }

		/// <summary>
		/// Идентификатор категории
		/// </summary>
		public int CategoryId { get; set; }

		/// <summary>
		/// Категория
		/// </summary>
		public  Category? Category { get; set; }

		/// <summary>
		/// Изображение товара
		/// </summary>
		public ICollection<ProductImage> Images { get; set; } = [];
	}
}
