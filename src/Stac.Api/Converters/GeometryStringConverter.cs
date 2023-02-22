using System;
using System.Linq;
using GeoJSON.Net.Converters;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Stac.Api.Models;
using Stac.Api.Models.Cql2;

namespace Stac.Api.Converters
{
    public class GeometryStringConverter : JsonConverter
    {
        GeometryConverter geomConverter = new GeometryConverter();

        public override bool CanConvert(Type objectType)
        {
            return typeof(IGeometryObject).IsAssignableFromType(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            IGeometryObject geometry = (IGeometryObject)geomConverter.ReadJson(reader, objectType, existingValue, serializer);
            if ( objectType == typeof(IGeometryObject) ) return geometry;
            return objectType.GetConstructor(new Type[] { typeof(IGeometryObject) }).Invoke(new object[] { geometry });
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var prop = value.GetType().GetProperties().FirstOrDefault(p => p.PropertyType == typeof(IGeometryObject));
            if ( prop == null ){
                throw new Exception($"{value.GetType().Name} does not have a property of type {typeof(IGeometryObject).Name}");
            }
            IGeometryObject geometry = (IGeometryObject)prop.GetValue(value, null);
            serializer.Serialize(writer, geometry);
        }
    }
}