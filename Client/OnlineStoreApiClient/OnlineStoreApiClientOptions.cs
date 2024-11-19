using Microsoft.Extensions.Configuration;

namespace OnlineStoreApiClients
{
    public sealed class OnlineStoreApiClientOptions
    {
        [ConfigurationKeyName("BaseUrl")]
        public string BaseUrl { get; set; }
    }
}
