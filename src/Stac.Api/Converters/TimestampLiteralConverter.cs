using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Api.Models.Cql2;

namespace Stac.Api.Converters
{
    internal class TimestampLiteralConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TimestampLiteral);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            reader.DateParseHandling = DateParseHandling.None;
            JObject jo = JObject.Load(reader);
            return new TimestampLiteral(jo["timestamp"].ToObject<DateTime>(serializer));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var timestamp = (TimestampLiteral)value;
            writer.WriteStartObject();
            writer.WritePropertyName("timestamp");
            writer.WriteValue(timestamp.DateTime.ToUniversalTime());
            writer.WriteEndObject();
        }
    }
}