using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stac.Api.Interfaces;

namespace Stac.Api.Clients.Collections
{
    public partial class StacCollections : IStacPageable<StacCollection>
    {
        public ICollection<StacCollection> Items => Collections;

        ICollection<StacLink> ILinksCollectionObject.Links => Links as ICollection<StacLink>;
    }
}