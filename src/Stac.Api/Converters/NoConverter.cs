using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Stac.Api.Converters
{
    public class NoConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            // Note - not called when attached directly via [JsonConverter(typeof(NoConverter))]
            throw new NotImplementedException();
        }

        public override bool CanRead { get { return false; } }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanWrite { get { return false; } }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}