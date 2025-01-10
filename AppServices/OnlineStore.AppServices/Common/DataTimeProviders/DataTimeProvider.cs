


namespace OnlineStore.AppServices.Common.DataTimeProviders
{
    /// <summary>
    /// Провайдера времени
    /// </summary>
    public sealed class DataTimeProvider : IDataTimeProvider
    {
        public DateTime UtcNow => DateTime.Now;
    }
}
