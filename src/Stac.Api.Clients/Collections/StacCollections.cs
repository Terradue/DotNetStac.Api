using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Stac.Api.Interfaces;

namespace Stac.Api.Clients.Collections
{
    public partial class StacCollections : ILinksCollectionObject
    {
        public StacCollections()
        {
            Collections = new List<StacCollection>();
        }

        public StacCollections(IEnumerable<StacCollection> collections)
        {
            Collections = collections.ToList();
        }

        [JsonProperty("numberMatched")]
        public int NumberMatched { get; set; }

        ICollection<StacLink> ILinksCollectionObject.Links => Links as ICollection<StacLink>;

    }
}