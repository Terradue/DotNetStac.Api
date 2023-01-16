using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GeoJSON.Net.Geometry;
using NetTopologySuite.Geometries;
using Newtonsoft.Json.Schema;

namespace Stac.Api.Interfaces
{
    public interface IStacQueryProvider : IQueryProvider
    {
        Expression GetFirstExpression();
        
        JSchema GetQueryables();

        IComparable GetStacObjectProperty<TSource>(TSource s, string property) where TSource : IStacObject;

        bool SpatialIntersects(Geometry geometry1 , Geometry geometry2);

        Geometry GetStacObjectGeometry<TSource>(TSource s, string property = "geometry") where TSource : IStacObject;

        Itenso.TimePeriod.ITimePeriod GetStacObjectDateTime<TSource>(TSource i, string v) where TSource : IStacObject;
    }
}