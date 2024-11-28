using System.ComponentModel;

namespace OnlineStore.Contracts.Enum
{
	/// <summary>
	/// Способы уведомления пользователя
	/// </summary>
	public enum NotificationChannelEnum
	{
		[Description("Email")]
		Email = 1,

		[Description("Telegram")]
		Telegram = 2,

		[Description("Личный кабинет")]
		PrivateCabinet = 3
	}
}
