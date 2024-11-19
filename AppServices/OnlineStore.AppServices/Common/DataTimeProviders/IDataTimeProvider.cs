
namespace OnlineStore.AppServices.Common.DataTimeProviders
{
    /// <summary>
    /// Интерфейс провайдера времени
    /// </summary>
    public interface IDataTimeProvider
    {
        /// <summary>
        /// Текущее время по UTC
        /// </summary>
        DateTime UtcNow { get; }
    }
}
