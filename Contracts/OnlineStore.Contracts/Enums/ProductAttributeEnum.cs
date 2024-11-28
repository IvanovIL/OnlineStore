using System.ComponentModel;

namespace OnlineStore.Contracts.Enum
{
	public enum ProductAttributeEnum
	{
		[Description("Используется для поиска")]
		ForSearch = 1,

		[Description("Не используется для поиска")]
		NotForSearch = 2
	}
}
