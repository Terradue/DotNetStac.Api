using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Stac.Api.Converters;
using Stac.Api.Interfaces;

namespace Stac.Api.Models.Core
{
    [JsonConverter(typeof(GeometryFilterConverter))]
    public class IntersectGeometryFilter : IGeometryFilter
    {
        public IGeometryObject Geometry { get; set; }

        public GeometryOperation Operation => GeometryOperation.Intersects;
    }
}