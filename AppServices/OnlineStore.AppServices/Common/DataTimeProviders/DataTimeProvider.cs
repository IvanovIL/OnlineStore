


namespace OnlineStore.AppServices.Common.DataTimeProviders
{
    /// <summary>
    /// Провайдера времени
    /// </summary>
    public sealed class DataTimeProvider : IDataTimeProvider
    {
        public DateTime Now => DateTime.Now;
    }
}
