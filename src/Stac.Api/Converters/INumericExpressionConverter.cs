using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Api.Models.Cql2;

namespace Stac.Api.Converters
{
    internal class INumericExpressionConverter : JsonConverter
    {
        NumberConverter numberConverter = new NumberConverter();

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(INumericExpression);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            reader.DateParseHandling = DateParseHandling.None;
            JToken jt = JToken.Load(reader);

            if (jt.Type == JTokenType.Float ||
                jt.Type == JTokenType.Integer)
            {
                return jt.ToObject<Number>(serializer);
            }
            if (jt is JObject jo)
            {
                return ReadJObject(jo, objectType, existingValue, serializer);
            }

            throw new JsonSerializationException($"Could not convert {jt.ToString()} to NumericExpression");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        internal INumericExpression ReadJObject(JObject jo, Type type, object existingValue, JsonSerializer serializer)
        {
            // property ref
            if (jo.ContainsKey("property"))
            {
                return jo.ToObject<PropertyRef>(serializer);
            }
            // function ref
            if (jo.ContainsKey("name"))
            {
                return jo.ToObject<FunctionRef>(serializer);
            }

            throw new JsonSerializationException($"Could not convert {jo.ToString()} to NumericExpression");

        }
    }
}