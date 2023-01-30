using System;
using GeoJSON.Net.Converters;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Stac.Api.Interfaces;

namespace Stac.Api.Converters
{
    public class GeometryFilterConverter<T> : JsonConverter<IGeometryFilter> where T : IGeometryFilter
    {
        private static GeometryConverter _geometryConverter = new GeometryConverter();

        public override IGeometryFilter ReadJson(JsonReader reader, Type objectType, IGeometryFilter existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            T filter = (T)Activator.CreateInstance(typeof(T));
            filter.Geometry = (IGeometryObject)_geometryConverter.ReadJson(reader, typeof(IGeometryObject), null, serializer);
            return filter;
        }

        public override void WriteJson(JsonWriter writer, IGeometryFilter value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.Geometry);
        }
    }
}