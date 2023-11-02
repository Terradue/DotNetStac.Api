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
    public class IsBetweenPredicateConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IsBetweenPredicate);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            reader.DateParseHandling = DateParseHandling.None;
            JObject jo = JObject.Load(reader);
            return ReadJObject(jo, objectType, existingValue, serializer);
        }

        public IsBetweenPredicate ReadJObject(JObject jo, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return new IsBetweenPredicate
            {
                Op = jo["op"].ToObject<IsBetweenPredicateOp>(serializer),
                Args = jo["args"].ToObject<IsBetweenOperands>()
            };
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var expression = (IsBetweenPredicate)value;
            writer.WriteStartObject();
            writer.WritePropertyName("op");
            serializer.Serialize(writer, expression.Op);
            writer.WritePropertyName("args");
            serializer.Serialize(writer, expression.Args);
            writer.WriteEndObject();
        }
    }


}