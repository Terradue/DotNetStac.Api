using System;
using GeoJSON.Net.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Api.Models;
using Stac.Api.Models.Cql2;

namespace Stac.Api.Converters
{
    public class BooleanExpressionConverter : JsonConverter
    {
        ComparisonPredicateConverter comparisonPredicateConverter = new ComparisonPredicateConverter();

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(BooleanExpression);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            reader.DateParseHandling = DateParseHandling.None;
            JObject jo = JObject.Load(reader);
            return ReadJObject(jo, objectType, existingValue, serializer);
        }

        public BooleanExpression ReadJObject(JObject jo, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (jo.ContainsKey("op"))
            {
                bool found = false;
                try
                {
                    jo["op"].ToObject<AndOrExpressionOp>(serializer);
                    found = true;
                }
                catch (JsonSerializationException) { }
                if (found)
                {
                    return serializer.Deserialize<AndOrExpression>(jo.CreateReader());
                }

                try
                {
                    jo["op"].ToObject<NotExpressionOp>(serializer);
                    found = true;
                }
                catch (JsonSerializationException) { }
                if (found)
                {
                    return jo.ToObject<NotExpression>(serializer);
                }

                return comparisonPredicateConverter.ReadJObject(jo, objectType, existingValue, serializer);
            }

            throw new JsonSerializationException($"Could not convert {jo.ToString()} to BooleanExpression");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }


}