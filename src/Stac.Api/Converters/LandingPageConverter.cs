using System;
using Newtonsoft.Json;
using Stac.Api.Models;

namespace Stac.Api.Converters
{
    internal class LandingPageConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(LandingPage);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return new LandingPage(StacConvert.Deserialize<IStacCatalog>(reader.ToString()));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue(StacConvert.Serialize(((LandingPage)value).StacCatalog));
        }
    }
}