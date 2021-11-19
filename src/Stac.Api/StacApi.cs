
using System;
using System.Net.Http;
using Stac.Api.Generated.Clients;

namespace Stac.Api
{
    public class StacApi
    {
        private readonly IServiceProvider _serviceProvider;
        private string _baseUrl;

        public StacApi(IServiceProvider serviceProvider, string baseUrl)
        {
            this._serviceProvider = serviceProvider;
            _baseUrl = baseUrl;
        }

        public string BaseUrl
        {
            get { return _baseUrl; }
            
        }

        public StacApiClient CoreClient
        {
            get { return null; }
        }

    }
}