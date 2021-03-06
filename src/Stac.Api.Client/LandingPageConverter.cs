using System;
using Newtonsoft.Json;

namespace Stac.Api.Client
{
    internal class LandingPageConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(LandingPage);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return StacConvert.Deserialize<IStacCatalog>(reader.ToString());
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue(StacConvert.Serialize((IStacCatalog)value));
        }
    }
}