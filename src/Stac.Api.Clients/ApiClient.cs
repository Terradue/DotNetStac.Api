using System;
using System.Net.Http;

namespace Stac.Api.Clients
{
    public sealed partial class ApiClient
    {
        private readonly HttpClient _client;

        public ApiClient(HttpClient client)
        {
            _client = client;
        }

        public ApiClient(string baseUrl)
        {
            _client = CreateHttpClient(baseUrl);
        }

        public ApiClient(Uri baseUri)
        {
            _client = CreateHttpClient(baseUri);
        }

        private static HttpClient? CreateHttpClient(string baseUrl)
        {
            return CreateHttpClient(new Uri(baseUrl));
        }

        private static HttpClient? CreateHttpClient(Uri baseUrl)
        {
            return new HttpClient
            {
                BaseAddress = baseUrl
            };
        }

        public string BaseUrl
        {
            get => _client.BaseAddress?.ToString() ?? string.Empty;
        }

    }
}