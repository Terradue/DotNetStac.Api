using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoJSON.Net.Geometry;
using NetTopologySuite.Geometries;

namespace Stac.Api.Extensions.Filters
{
    public static class GeometryExtensions
    {
        public static bool Intersects(this IGeometryObject geometry, double[] bbox)
        {
            NetTopologySuite.Geometries.Geometry bboxGeometry = new NetTopologySuite.Geometries.GeometryFactory().ToGeometry(new Envelope(bbox[0], bbox[2], bbox[1], bbox[3]));
            NetTopologySuite.Geometries.Geometry geometry2 = geometry.ToNTSGeometry();
            return geometry2.Intersects(bboxGeometry);
        }

        public static bool Intersects(this IGeometryObject geometry, IGeometryObject geometry2)
        {
            NetTopologySuite.Geometries.Geometry geometry3 = geometry.ToNTSGeometry();
            NetTopologySuite.Geometries.Geometry geometry4 = geometry2.ToNTSGeometry();
            return geometry3.Intersects(geometry4);
        }
    }
}