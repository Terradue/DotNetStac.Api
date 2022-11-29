using Newtonsoft.Json;
using Stac.Api.Converters;

namespace Stac.Api.Models
{
    [JsonConverter(typeof(PostStacItemOrCollectionConverter))]
    public class PostStacItemOrCollection
    {
        [JsonConstructor]
        public PostStacItemOrCollection(object obj)
        {
            if (obj is StacItem stacItem)
            {
                StacItem = stacItem;
            }
            else if (obj is StacFeatureCollection stacFeatureCollection)
            {
                StacFeatureCollection = stacFeatureCollection;
            }
        }

        public bool IsCollection => StacFeatureCollection != null;

        public StacItem StacItem { get; }

        public StacFeatureCollection StacFeatureCollection { get; }
    }
}