using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Stac.Api.Interfaces
{
    public interface IDataServicesProvider
    {
        IRootCatalogProvider GetRootCatalogProvider(Microsoft.AspNetCore.Http.HttpContext httpContext);
        ICollectionsProvider GetCollectionsProvider(Microsoft.AspNetCore.Http.HttpContext httpContext);
        IItemsBroker GetItemsBroker(string collectionId, HttpContext httpContext);
        IItemsProvider GetItemsProvider(string collectionId, HttpContext httpContext);
    }
}