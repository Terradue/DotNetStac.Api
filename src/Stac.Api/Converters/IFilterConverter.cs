using System;
using GeoJSON.Net.Converters;
using Newtonsoft.Json;
using Stac.Api.Interfaces;

namespace Stac.Api.Converters
{
    internal class IFilterConverter : JsonConverter
    {
        BooleanExpressionConverter booleanExpressionConverter = new BooleanExpressionConverter();

        public override bool CanConvert(Type objectType)
        {
            return typeof(IStacFilter).IsAssignableFromType(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return booleanExpressionConverter.ReadJson(reader, objectType, existingValue, serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }


}