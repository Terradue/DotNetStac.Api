using System;
using GeoJSON.Net.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Api.Models;
using Stac.Api.Models.Cql2;

namespace Stac.Api.Converters
{
    public class IIsNullOperandConverter : JsonConverter
    {
        CharExpressionConverter charExpressionConverter = new CharExpressionConverter();
        NumberConverter numericExpressionConverter = new NumberConverter();
        BooleanExpressionConverter booleanExpressionConverter = new BooleanExpressionConverter();
        ITemporalExpressionConverter temporalExpressionConverter = new ITemporalExpressionConverter();
        IGeomExpressionConverter geometryExpressionConverter = new IGeomExpressionConverter();

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IIsNullOperand);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            reader.DateParseHandling = DateParseHandling.None;
            JObject jo = JObject.Load(reader);
            return ReadJObject(jo, objectType, existingValue, serializer);
        }

        private IIsNullOperand ReadJObject(JObject jo, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // numeric
            try
            {
                return numericExpressionConverter.ReadJObject(jo, typeof(Number), existingValue, serializer);
            }
            catch { }
            // boolean
            try
            {
                return booleanExpressionConverter.ReadJObject(jo, typeof(BoolExpression), existingValue, serializer);
            }
            catch { }
            // char
            try
            {
                return charExpressionConverter.ReadJObject(jo, typeof(CharExpression), existingValue, serializer);
            }
            catch { }
            // temporal
            try
            {
                return temporalExpressionConverter.ReadJObject(jo, typeof(ITemporalInstantExpression), existingValue, serializer);
            }
            catch { }
            // geom
            try
            {
                return geometryExpressionConverter.ReadJObject(jo, typeof(ITemporalInstantExpression), existingValue, serializer);
            }
            catch { }

            throw new JsonSerializationException($"Could not convert {jo.ToString()} to IsNullOperand");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }


}