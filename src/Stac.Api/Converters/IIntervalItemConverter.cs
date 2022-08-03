using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Api.Models;
using Stac.Api.Models.Cql2;

namespace Stac.Api.Converters
{
    internal class IIntervalItemConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IIntervalItem);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            reader.DateParseHandling = DateParseHandling.None;
            JToken jt = JToken.Load(reader);

            if (jt.Type is JTokenType.String)
            {
                // simplest case string '..'
                if (jt.Value<string>() == "..")
                {
                    return new StringIntervalItem(StringIntervalItemEnum.DotDot);
                }
                // Date
                try
                {
                    return DateString.Parse(jt.Value<string>());
                }
                catch { }
                // DateTime
                try
                {
                    return TimestampString.Parse(jt.Value<string>());
                }
                catch { }
            }

            if (jt is JObject jo)
            {
                // Property reference
                if (jo.ContainsKey("property"))
                {
                    return jo.ToObject<PropertyRef>();
                }

                // Function reference
                if (jo.ContainsKey("name"))
                {
                    return jo.ToObject<FunctionRef>();
                }
            }

            throw new JsonSerializationException($"Could not convert {jt.ToString()} to IntervalItem"); 
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            switch (value)
            {
                case IInstantString iis:
                    writer.WriteRawValue(iis.ToString());
                    break;
                case PropertyRef pr:
                    serializer.Serialize(writer, pr);
                    break;
                case FunctionRef fr:
                    serializer.Serialize(writer, fr);
                    break;
                default:
                    throw new FormatException("Invalid interval item : " + value.ToString());
            }
        }
    }


}