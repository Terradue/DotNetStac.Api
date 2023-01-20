using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Stac.Api.Interfaces;

namespace Stac.Api.Clients.Collections
{
    public partial class StacCollections :ILinksCollectionObject
    {

        ICollection<StacLink> ILinksCollectionObject.Links => Links as ICollection<StacLink>;
    }
}