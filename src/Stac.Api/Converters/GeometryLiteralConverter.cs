using System;
using GeoJSON.Net.Converters;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Stac.Api.Models;
using Stac.Api.Models.Cql2;

namespace Stac.Api.Converters
{
    public class GeometryLiteralConverter : JsonConverter
    {
        GeometryConverter geomConverter = new GeometryConverter();

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(GeometryLiteral);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            IGeometryObject geometry = (IGeometryObject)geomConverter.ReadJson(reader, objectType, existingValue, serializer);
            return new GeometryLiteral(geometry);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            GeometryLiteral geometry = (GeometryLiteral)value;
            geomConverter.WriteJson(writer, geometry.GeometryObject, serializer);
        }
    }
}