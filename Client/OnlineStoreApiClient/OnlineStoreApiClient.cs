using OnlineStore.Contracts.Product;
using OnlineStore.Infrastructure.JwtGenerator;
using System.ComponentModel.Design;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace OnlineStoreApiClients
{
    public sealed class OnlineStoreApiClient : IOnlineStoreApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _jwtToken;
        private readonly IJwtGenerator _jwtGenerator;


        public OnlineStoreApiClient(HttpClient httpClient,
            IJwtGenerator jwtGenerator)
        {
            _httpClient = httpClient;
            _jwtToken = jwtGenerator.GenerateToken();
        }

        public Task AddProductAsync(ShortProductDto productDto, CancellationToken cancellation)
        {
            
            return PostAsync("Products/Add/product", productDto, cancellation);
        }

        private async Task<T> GetAsync<T>(string requestUri, CancellationToken cancellation)
        {
            using var httpRequestMessage = CreatRequest(HttpMethod.Get, null, requestUri); ;
            var response = await _httpClient.SendAsync(httpRequestMessage, cancellation);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }

        private async Task<TResponse> PostAsync<TResponse>(string requestUri, object body, CancellationToken cancellation)
        {
            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var httpRequest = CreatRequest(HttpMethod.Post, content, requestUri);
            var response = await _httpClient.SendAsync(httpRequest, cancellation);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.StatusCode.ToString());
            }

            return await response.Content.ReadFromJsonAsync<TResponse>(cancellation);


        }

        private async Task PostAsync(string requestUri, object body, CancellationToken cancellation)
        {
            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var httpRequest = CreatRequest(HttpMethod.Post, content, requestUri);
            var response = await _httpClient.SendAsync(httpRequest, cancellation);

            response.EnsureSuccessStatusCode();
        }

        private HttpRequestMessage CreatRequest(HttpMethod method, HttpContent content, string uri)
        {
            var request = new HttpRequestMessage(method, uri);

            if (content != null)
            {
                request.Content = content;
            }

            request.Headers.Authorization = CreateAuthorizationHeaderValue(_jwtToken);
            return request;
        }

        private static AuthenticationHeaderValue CreateAuthorizationHeaderValue(string jwtToken) =>
            new AuthenticationHeaderValue("Bearer", jwtToken);


    }
}
