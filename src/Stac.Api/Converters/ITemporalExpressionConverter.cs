using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Api.Models.Cql2;

namespace Stac.Api.Converters
{
    internal class ITemporalExpressionConverter : JsonConverter
    {
        ITemporalLiteralConverter temporalLiteralConverter = new ITemporalLiteralConverter();

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ITemporalExpression);
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

        internal ITemporalExpression ReadJObject(JObject jo, Type type, object existingValue, JsonSerializer serializer)
        {
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

            return temporalLiteralConverter.ReadJObject(jo, type, existingValue, serializer);

        }
    }
}