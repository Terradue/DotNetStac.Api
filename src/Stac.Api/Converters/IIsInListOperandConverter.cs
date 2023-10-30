using System;
using System.Linq;
using GeoJSON.Net.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Api.Models;
using Stac.Api.Models.Cql2;

namespace Stac.Api.Converters
{
    public class IIsInListOperandConverter : JsonConverter
    {
        ScalarExpressionConverter scalarExpressionConverter = new ScalarExpressionConverter();

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IIsNullOperand);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            reader.DateParseHandling = DateParseHandling.None;
            JToken jt = JToken.Load(reader);

            if (jt.Type == JTokenType.Array)
            {
                var ja = (JArray)jt;
                return new ScalarExpressionCollection(ja.Select(j => scalarExpressionConverter.ReadJToken(j, objectType, existingValue, serializer)).Cast<IScalarExpression>().ToList());
            }
            else {
                return scalarExpressionConverter.ReadJToken(jt, objectType, existingValue, serializer);
            }

            throw new JsonSerializationException($"Could not convert {jt.ToString()} to IIsInListOperand");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }


}