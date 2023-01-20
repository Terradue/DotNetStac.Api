using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Stac.Api.Converters
{
    public class ListConverter<T, T2> : JsonConverter where T : T2
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(List<T>));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            if (token.Type == JTokenType.Array)
            {
                var items = token.ToObject<List<T>>();
                return new List<T2>(items.Cast<T2>());
            }
            return new List<T2>();
        }

        public override bool CanRead => true;

        public override bool CanWrite => true;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            List<T2> collection = (List<T2>)value;
            writer.WriteStartArray();
            foreach (var item in collection)
            {
                serializer.Serialize(writer, item);
            }
            writer.WriteEndArray();
        }
    }
}