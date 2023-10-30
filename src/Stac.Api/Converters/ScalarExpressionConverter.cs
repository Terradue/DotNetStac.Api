using System;
using GeoJSON.Net.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Api.Models;
using Stac.Api.Models.Cql2;

namespace Stac.Api.Converters
{
    public class ScalarExpressionConverter : JsonConverter
    {
        CharExpressionConverter charExpressionConverter = new CharExpressionConverter();
        NumberConverter numericExpressionConverter = new NumberConverter();
        BoolExpressionConverter boolExpressionConverter = new BoolExpressionConverter();
        TemporalInstantExpressionConverter temporalInstantExpressionConverter = new TemporalInstantExpressionConverter();

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IScalarExpression);
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
            
            try
            {
                double value = serializer.Deserialize<double>(reader);
                return new Number(value);
            }
            catch (JsonSerializationException) { }

            try
            {
                bool value = serializer.Deserialize<bool>(reader);
                return new BoolExpression(value);
            }
            catch (JsonSerializationException) { }
            
            try
            {
                string value = serializer.Deserialize<string>(reader);
                return new Models.Cql2.String(value);
            }
            catch (JsonSerializationException) { }

            throw new JsonSerializationException($"Could not convert {reader.Value} to ScalarExpression"); 
        }

        public object ReadJToken(JToken jt, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if ( jt.Type == JTokenType.Object)
                return ReadJObject((JObject)jt, objectType, existingValue, serializer);

            try
            {
                return new Number(jt.Value<double>());
            }
            catch { }

            try
            {
                return new BoolExpression(jt.Value<bool>());
            }
            catch { }
            
            try
            {
                return new Models.Cql2.String(jt.Value<string>());
            }
            catch { }

            throw new JsonSerializationException($"Could not convert {jt.ToString()} to ScalarExpression"); 
        }

        public IScalarExpression ReadJObject(JObject jo, Type objectType, object existingValue, JsonSerializer serializer)
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
                return boolExpressionConverter.ReadJObject(jo, typeof(BoolExpression), existingValue, serializer);
            }
            catch { }
            // char
            try
            {
                return charExpressionConverter.ReadJObject(jo, typeof(CharExpression), existingValue, serializer);
            }
            catch { }
            // temporal instant
            try
            {
                return temporalInstantExpressionConverter.ReadJObject(jo, typeof(ITemporalInstantExpression), existingValue, serializer);
            }
            catch { }

            throw new JsonSerializationException($"Could not convert {jo.ToString()} to ScalarExpression");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }


}