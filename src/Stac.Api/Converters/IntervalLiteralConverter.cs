using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Api.Models.Cql2;

namespace Stac.Api.Converters
{
    internal class IntervalLiteralConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TimestampLiteral);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            reader.DateParseHandling = DateParseHandling.DateTimeOffset;
            JObject jo = JObject.Load(reader);
            return new IntervalLiteral(jo["interval"].ToObject<IntervalArray>(serializer));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var interval = (IntervalLiteral)value;
            writer.WriteStartObject();
            writer.WritePropertyName("interval");
            serializer.Serialize(writer, interval.Interval.Select(i => i.DateTime).ToArray());
            writer.WriteEndObject();
        }
    }
}