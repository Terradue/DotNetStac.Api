
using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

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

        // public CoreClient Core()
        // {
        //     return _serviceProvider.GetService<CoreClient>();
        // }

    }
}