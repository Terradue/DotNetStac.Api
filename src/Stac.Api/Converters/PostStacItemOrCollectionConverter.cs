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
                return jo.ToObject<StacItem>(serializer);
            }

            // Stac Features Collection
            return jo.ToObject<StacFeatureCollection>(serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}