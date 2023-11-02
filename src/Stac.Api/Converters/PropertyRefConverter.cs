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
    public class PropertyRefConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PropertyRef);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            reader.DateParseHandling = DateParseHandling.None;
            JObject jo = JObject.Load(reader);
            return ReadJObject(jo, objectType, existingValue, serializer);
        }

        public PropertyRef ReadJObject(JObject jo, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return new PropertyRef
            {
                Property = jo["property"].ToObject<string>(serializer)
            };
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var expression = (PropertyRef)value;
            writer.WriteStartObject();
            writer.WritePropertyName("property");
            serializer.Serialize(writer, expression.Property);
            writer.WriteEndObject();
        }
    }


}