using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            JObject jo = JObject.Load(reader);
            string json = jo.ToString(Formatting.None);
            return new LandingPage(StacConvert.Deserialize<StacCatalog>(json));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, ((LandingPage)value).StacCatalog, typeof(StacCatalog));
        }
    }
}