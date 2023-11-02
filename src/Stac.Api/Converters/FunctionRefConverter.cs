using System;
using System.Collections.Generic;
using System.Linq;
using GeoJSON.Net.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Api.Models;
using Stac.Api.Models.Cql2;

namespace Stac.Api.Converters
{
    public class FunctionRefConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(FunctionRef);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            reader.DateParseHandling = DateParseHandling.None;
            JObject jo = JObject.Load(reader);
            return ReadJObject(jo, objectType, existingValue, serializer);
        }

        public FunctionRef ReadJObject(JObject jo, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return new FunctionRef
            {
                Function = jo["function"].ToObject<Function>(serializer)
            };
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var expression = (FunctionRef)value;
            writer.WriteStartObject();
            writer.WritePropertyName("function");
            serializer.Serialize(writer, expression.Function);
            writer.WriteEndObject();
        }
    }


}