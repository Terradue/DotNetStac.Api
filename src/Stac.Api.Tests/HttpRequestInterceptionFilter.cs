using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JustEat.HttpClientInterception;
using Microsoft.Extensions.Http;

namespace Stac.Api.Tests
{
    public sealed class HttpRequestInterceptionFilter : IHttpMessageHandlerBuilderFilter
    {
        private readonly HttpClientInterceptorOptions _options;

        internal HttpRequestInterceptionFilter(HttpClientInterceptorOptions options)
        {
            _options = options;
        }

        public Action<HttpMessageHandlerBuilder> Configure(Action<HttpMessageHandlerBuilder> next)
        {
            return builder =>
            {
                next(builder);
                builder.AdditionalHandlers.Add(_options.CreateHttpMessageHandler());
            };
        }
    }
}