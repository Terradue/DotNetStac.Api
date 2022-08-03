using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Api.Models.Cql2;

namespace Stac.Api.Converters
{
    internal class BoolExpressionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(BoolExpression);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return new BoolExpression(reader.ReadAsBoolean().Value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, ((BoolExpression)value).Bool);
        }

        public BoolExpression ReadJObject(JObject jo, Type type, object existingValue, JsonSerializer serializer)
        {
            return new BoolExpression(jo.Value<bool>());
        }
    }
}