using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Api.Models;

namespace Stac.Api.Converters
{
    public class PostStacItemOrCollectionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PostStacItemOrCollection);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            reader.DateParseHandling = DateParseHandling.None;
            JObject jo = JObject.Load(reader);

            // Stac Item
            if (jo.ContainsKey("type") && jo["type"].ToString() == "Feature")
            {
                return new PostStacItemOrCollection(jo.ToObject<StacItem>(serializer));
            }

            // Stac Features Collection
            return new PostStacItemOrCollection(jo.ToObject<StacFeatureCollection>(serializer));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if ( value is PostStacItemOrCollection postStacItemOrCollection)
            {
                if (postStacItemOrCollection.StacItem != null)
                {
                    serializer.Serialize(writer, postStacItemOrCollection.StacItem);
                }
                else if (postStacItemOrCollection.StacFeatureCollection != null)
                {
                    serializer.Serialize(writer, postStacItemOrCollection.StacFeatureCollection);
                }
            }
            else
            {
                throw new Exception("Unexpected value when converting PostStacItemOrCollection");
            }
        }
    }
}