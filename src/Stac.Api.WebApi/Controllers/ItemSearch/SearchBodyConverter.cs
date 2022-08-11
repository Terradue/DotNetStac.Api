using System;
using GeoJSON.Net.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Api.Models.Cql2;

namespace Stac.Api.WebApi.Controllers.ItemSearch
{
    internal class SearchRequestConverter : JsonConverter
    {
        GeometryConverter geometryConverter = new GeometryConverter();

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(SearchRequest);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            reader.DateParseHandling = DateParseHandling.None;
            JObject jo = JObject.Load(reader);
            return ReadJObject(jo, objectType, existingValue, serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        internal ISpatialLiteral ReadJObject(JObject jo, Type type, object existingValue, JsonSerializer serializer)
        {
            // envelope
            if (jo.ContainsKey("bbox"))
            {
                return jo.ToObject<EnvelopeLiteral>(serializer);
            }

            return jo.ToObject<GeometryLiteral>(serializer);
        }
    }
}