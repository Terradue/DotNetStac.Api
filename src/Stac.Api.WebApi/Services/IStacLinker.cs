using Microsoft.AspNetCore.Http;
using Stac.Api.Clients.Collections;
using Stac.Api.Interfaces;
using Stac.Api.Models;

namespace Stac.Api.WebApi.Services
{
    public interface IStacLinker
    {
        void Link(LandingPage landingPage, IStacApiContext stacApiContext);

        void Link(StacCollection collection, IStacApiContext stacApiContext);

        void Link(StacCollections collections, IStacApiContext stacApiContext);

        void Link(StacItem item, IStacApiContext stacApiContext);

        void Link(StacFeatureCollection collection, IStacApiContext stacApiContext);

        void Link<T>(T linksCollectionObject, IStacLinkValuesProvider<T> stacLinkValuesProvider, IStacApiContext stacApiContext) where T : ILinksCollectionObject;
        
    }
}