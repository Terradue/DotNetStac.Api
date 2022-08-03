using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Api.Models.Cql2;

namespace Stac.Api.Converters
{
    internal class ITemporalLiteralConverter : JsonConverter
    {
        GeometryLiteralConverter geometryLiteralConverter = new GeometryLiteralConverter();

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ITemporalLiteral);
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

        internal ITemporalLiteral ReadJObject(JObject jo, Type type, object existingValue, JsonSerializer serializer)
        {
            // envelope
            if (jo.ContainsKey("interval"))
            {
                return jo.ToObject<IntervalLiteral>(serializer);
            }

            return jo.ToObject<InstantLiteral>(serializer);
        }
    }
}