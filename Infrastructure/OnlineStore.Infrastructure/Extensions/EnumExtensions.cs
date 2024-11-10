using System.ComponentModel;
using System.Reflection;

namespace OnlineStore.Infrastructure.Extensions
{
	public  static class EnumExtensions
	{
		public static string GetEnumDescription(this Enum value)
		{
			var field = value.GetType().GetField(value.ToString());
			var attrubute = field.GetCustomAttribute<DescriptionAttribute>();
			return attrubute == null ? value.ToString() : attrubute.Description;
		}
	}
}
