using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Api.Models.Cql2;

namespace Stac.Api.Converters
{
    internal class CharExpressionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(CharExpression);
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

        internal CharExpression ReadJObject(JObject jo, Type type, object existingValue, JsonSerializer serializer)
        {
            // casei
            if ( jo.ContainsKey("casei") )
            {
                return jo.ToObject<CaseiExpression>(serializer);
            }
            // accenti
            if ( jo.ContainsKey("accenti") )
            {
                return jo.ToObject<AccentiExpression>(serializer);
            }
            // property ref
            if ( jo.ContainsKey("property") )
            {
                return jo.ToObject<PropertyRef>(serializer);
            }
            // function ref
            if ( jo.ContainsKey("name") )
            {
                return jo.ToObject<FunctionRef>(serializer);
            }

            throw new JsonSerializationException($"Could not convert {jo.ToString()} to CharExpression");
        }
    }
}