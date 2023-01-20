using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Stac.Api.Interfaces
{
    public interface IDataServicesProvider
    {
        IRootCatalogProvider GetRootCatalogProvider();
        ICollectionsProvider GetCollectionsProvider();
        IItemsBroker GetItemsBroker();
        IItemsProvider GetItemsProvider();
    }
}