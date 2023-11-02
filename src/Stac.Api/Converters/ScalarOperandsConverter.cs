using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Api.Models;
using Stac.Api.Models.Cql2;

namespace Stac.Api.Converters
{
    internal class ScalarOperandsConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ScalarOperands);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            reader.DateParseHandling = DateParseHandling.None;
            try
            {
                JArray array = JArray.Load(reader);
                return new ScalarOperands(array.ToObject<IScalarExpression[]>());
            }
            catch { }

            throw new JsonSerializationException($"Could not convert {reader.Value} to ScalarOperands");

        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var operands = (ScalarOperands)value;
            writer.WriteStartArray();
            foreach (var operand in operands)
            {
                serializer.Serialize(writer, operand);
            }
            writer.WriteEndArray();
        }
    }


}