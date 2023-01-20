using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Stac.Api.Interfaces;

namespace Stac.Api.WebApi.Services.Context
{
    public class HttpStacApiContext : IStacApiContext
    {
        public Uri BaseUri { get; private set; }

        public LinkGenerator LinkGenerator { get; private set; }

        public HttpContext HttpContext { get; private set; }

        public IPaginationParameters PaginationParameters { get; private set; }
        
        public string Collection { get; private set; }
        
        public int MatchedItemsCount { get; private set; }

        public static HttpStacApiContext Create(HttpContext httpContext)
        {
            return new HttpStacApiContext
            {
                BaseUri = new Uri(httpContext.Request.Scheme + "://" + httpContext.Request.Host),
                LinkGenerator = httpContext.RequestServices.GetService(typeof(LinkGenerator)) as LinkGenerator,
                HttpContext = httpContext,
                PaginationParameters = QueryStringPaginationParameters.GetPaginatorParameters(httpContext)
            };
        }

        public void SetCollection(string collectionId)
        {
            Collection = collectionId;
        }

        public void SetMatchedItemsCount(int count)
        {
            MatchedItemsCount = count;
        }

        public void SetPaginationParameters(IPaginationParameters paginationParameters)
        {
            PaginationParameters = paginationParameters;
        }
    }
}