using Microsoft.AspNetCore.Http;
using Stac.Api.Clients.Collections;

namespace Stac.Api.WebApi.Services
{
    public interface IStacLinker
    {
        void Link(StacCollection collection, HttpContext httpContext);

        void Link(StacCollections collections, HttpContext httpContext);

        void Link(StacItem item, HttpContext httpContext);

        void Link<T>(T linksCollectionObject, IStacLinkValuesProvider<T> stacLinkValuesProvider, HttpContext httpContext) where T : ILinksCollectionObject;
        
    }
}