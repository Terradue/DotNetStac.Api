using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GeoJSON.Net.Geometry;
using NetTopologySuite.Geometries;
using Newtonsoft.Json.Schema;
using Stac.Api.Services.Queryable;

namespace Stac.Api.FileSystem.Services
{
    public class FileSystemStacQueryProvider : StacQueryProvider
    {
        public FileSystemStacQueryProvider(StacQueryablesOptions queryablesOptions) : base(queryablesOptions)
        {
        }

        public override TResult Execute<TResult>(Expression expression)
        {

            throw new NotImplementedException();
        }

        public override Expression GetFirstExpression()
        {
            throw new NotImplementedException();
        }

        public override Geometry GetStacObjectGeometry<TSource>(TSource s, string property = "geometry")
        {
            throw new NotImplementedException();
        }

        public override bool SpatialIntersects(Geometry geometry1, Geometry geometry2)
        {
            throw new NotImplementedException();
        }
    }
}