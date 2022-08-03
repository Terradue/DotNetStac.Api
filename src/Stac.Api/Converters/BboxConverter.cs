using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Api.Models;
using Stac.Api.Models.Cql2;

namespace Stac.Api.Converters
{
    internal class BboxConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Bbox);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            reader.DateParseHandling = DateParseHandling.None;
            JObject jo = JObject.Load(reader);
            if (jo.Type == JTokenType.Array)
            {
                JArray ja = jo.ToObject<JArray>();
                if ( ja.Count == 4 )
                {
                    return new Bbox(ja[0].Value<double>(), ja[1].Value<double>(), ja[2].Value<double>(), ja[3].Value<double>());
                }
                if ( ja.Count == 6 )
                {
                    return new Bbox(ja[0].Value<double>(), ja[1].Value<double>(), ja[2].Value<double>(), ja[3].Value<double>(), ja[4].Value<double>(), ja[5].Value<double>());
                }
            }
            throw new FormatException("Invalid bbox : " + jo.Value<string>());
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value as System.Collections.Generic.List<double>);
        }
    }
}