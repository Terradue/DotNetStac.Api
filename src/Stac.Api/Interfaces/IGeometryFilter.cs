using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoJSON.Net.Geometry;

namespace Stac.Api.Interfaces
{
    public interface IGeometryFilter : IConvertible
    {
        IGeometryObject Geometry { get; set; }

        GeometryOperation Operation { get; }
    }
}