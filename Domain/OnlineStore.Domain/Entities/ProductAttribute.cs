using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.Domain.Entities
{
	/// <summary>
	/// Атрибут категории
	/// </summary>
	[Table("Attributes")]
	public sealed class ProductAttribute
	{
		/// <summary>
		/// Идентификатор 
		/// </summary>
		[Key]
		[Column("id")]
		public int Id { get; set; }

		/// <summary>
		/// Наименование категории
		/// </summary>
		[Column("Name")]
		public string Name { get; set; }
	}
}
