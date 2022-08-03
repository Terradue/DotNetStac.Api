using System;
using GeoJSON.Net.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Api.Models;
using Stac.Api.Models.Cql2;

namespace Stac.Api.Converters
{
    public class TemporalInstantExpressionConverter : JsonConverter
    {

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ITemporalInstantExpression);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            reader.DateParseHandling = DateParseHandling.None;
            try
            {
                JObject jo = JObject.Load(reader);
                return ReadJObject(jo, objectType, existingValue, serializer);
            }
            catch (JsonReaderException) { }

            throw new JsonSerializationException($"Could not convert {reader.Value} to TemporalInstantExpression"); 
        }

        public ITemporalInstantExpression ReadJObject(JObject jo, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // timestamp
            if ( jo.ContainsKey("timestamp") )
            {
                return jo.ToObject<TimestampLiteral>(serializer);
            }
            // boolean
            if ( jo.ContainsKey("date") )
            {
                return jo.ToObject<DateLiteral>(serializer);
            }

            throw new JsonSerializationException($"Could not convert {jo.ToString()} to TemporalInstantExpression");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }


}