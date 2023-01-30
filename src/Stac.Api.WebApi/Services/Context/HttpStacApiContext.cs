using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Stac.Api.Interfaces;

namespace Stac.Api.WebApi.Services.Context
{
    public class HttpStacApiContext : IStacApiContext
    {
        public Uri BaseUri { get; private set; }

        public LinkGenerator LinkGenerator { get; private set; }

        public HttpContext HttpContext { get; private set; }

        public IList<string> Collections { get; private set; }
        
        public int MatchedItemsCount { get; private set; }

        public IList<ILinkValues> LinkValues { get; } = new List<ILinkValues>();

        public IDictionary<string, object> Properties { get; } = new Dictionary<string, object>();

        public IStacObject StacObjectContainer => throw new NotImplementedException();

        public static HttpStacApiContext Create(HttpContext httpContext)
        {
            return new HttpStacApiContext
            {
                BaseUri = new Uri(httpContext.Request.Scheme + "://" + httpContext.Request.Host),
                LinkGenerator = httpContext.RequestServices.GetService(typeof(LinkGenerator)) as LinkGenerator,
                HttpContext = httpContext,
            };
        }

        public void SetCollections(IList<string> collectionIds)
        {
            Collections = collectionIds;
        }

        public void SetMatchedItemsCount(int count)
        {
            MatchedItemsCount = count;
        }
    }
}