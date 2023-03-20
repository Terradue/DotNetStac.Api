using System;
using System.Net.Http;

namespace Stac.Api.Clients
{
    public abstract class ApiClient
    {
        private readonly HttpClient _client;

        protected ApiClient(HttpClient client)
        {
            _client = client;
        }

        protected ApiClient(string baseUrl)
        {
            _client = CreateHttpClient(baseUrl);
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