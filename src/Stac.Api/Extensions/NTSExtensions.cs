using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;

namespace Stac.Api.Extensions
{
    public static class NTSExtensions
    {
        public static NetTopologySuite.Geometries.Geometry ToNTSGeometry(this GeoJSON.Net.Geometry.IGeometryObject geometryObject)
        {
            switch (geometryObject.Type)
            {
                case GeoJSON.Net.GeoJSONObjectType.Point:
                    return ((GeoJSON.Net.Geometry.Point)geometryObject).ToNTSPoint();
                case GeoJSON.Net.GeoJSONObjectType.MultiPoint:
                    return ((GeoJSON.Net.Geometry.MultiPoint)geometryObject).ToNTSMultiPoint();
                case GeoJSON.Net.GeoJSONObjectType.LineString:
                    return ((GeoJSON.Net.Geometry.LineString)geometryObject).ToNTSLineString();
                case GeoJSON.Net.GeoJSONObjectType.MultiLineString:
                    return ((GeoJSON.Net.Geometry.MultiLineString)geometryObject).ToNTSMultiLineString();
                case GeoJSON.Net.GeoJSONObjectType.Polygon:
                    return ((GeoJSON.Net.Geometry.Polygon)geometryObject).ToNTSPolygon();
                case GeoJSON.Net.GeoJSONObjectType.MultiPolygon:
                    return ((GeoJSON.Net.Geometry.MultiPolygon)geometryObject).ToNTSMultiPolygon();
                case GeoJSON.Net.GeoJSONObjectType.GeometryCollection:
                    return ((GeoJSON.Net.Geometry.GeometryCollection)geometryObject).ToNTSGeometryCollection();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static NetTopologySuite.Geometries.Point ToNTSPoint(this GeoJSON.Net.Geometry.Point geometryPoint)
        {
            return new NetTopologySuite.Geometries.Point(geometryPoint.Coordinates.Longitude, geometryPoint.Coordinates.Latitude, geometryPoint.Coordinates.Altitude ?? 0);
        }

        public static NetTopologySuite.Geometries.Coordinate ToNTSCoordinate(this GeoJSON.Net.Geometry.IPosition geometryPosition)
        {
            var coordinate = new NetTopologySuite.Geometries.Coordinate(geometryPosition.Longitude, geometryPosition.Latitude);
            if (geometryPosition.Altitude.HasValue)
                coordinate.Z = geometryPosition.Altitude.Value;
            return coordinate;
        }

        public static NetTopologySuite.Geometries.MultiPoint ToNTSMultiPoint(this GeoJSON.Net.Geometry.MultiPoint geometryMultiPoint)
        {
            return new NetTopologySuite.Geometries.MultiPoint(geometryMultiPoint.Coordinates.Select(c => c.ToNTSPoint()).ToArray());
        }

        public static NetTopologySuite.Geometries.LineString ToNTSLineString(this GeoJSON.Net.Geometry.LineString geometryLineString)
        {
            return new NetTopologySuite.Geometries.LineString(geometryLineString.Coordinates.Select(c => c.ToNTSCoordinate()).ToArray());
        }

        public static NetTopologySuite.Geometries.MultiLineString ToNTSMultiLineString(this GeoJSON.Net.Geometry.MultiLineString geometryMultiLineString)
        {
            return new NetTopologySuite.Geometries.MultiLineString(geometryMultiLineString.Coordinates.Select(c => c.ToNTSLineString()).ToArray());
        }

        public static NetTopologySuite.Geometries.LinearRing ToNTSLinearRing(this GeoJSON.Net.Geometry.LineString geometryLineString)
        {
            return new NetTopologySuite.Geometries.LinearRing(geometryLineString.Coordinates.Select(c => c.ToNTSCoordinate()).ToArray());
        }

        public static NetTopologySuite.Geometries.Polygon ToNTSPolygon(this GeoJSON.Net.Geometry.Polygon geometryPolygon)
        {
            return new NetTopologySuite.Geometries.Polygon(geometryPolygon.Coordinates.First().ToNTSLinearRing(), geometryPolygon.Coordinates.Skip(1).Select(c => c.ToNTSLinearRing()).ToArray());
        }

        public static NetTopologySuite.Geometries.MultiPolygon ToNTSMultiPolygon(this GeoJSON.Net.Geometry.MultiPolygon geometryMultiPolygon)
        {
            return new NetTopologySuite.Geometries.MultiPolygon(geometryMultiPolygon.Coordinates.Select(c => c.ToNTSPolygon()).ToArray());
        }

        public static NetTopologySuite.Geometries.GeometryCollection ToNTSGeometryCollection(this GeoJSON.Net.Geometry.GeometryCollection geometryCollection)
        {
            return new NetTopologySuite.Geometries.GeometryCollection(geometryCollection.Geometries.Select(g => g.ToNTSGeometry()).ToArray());
        }
    }
}