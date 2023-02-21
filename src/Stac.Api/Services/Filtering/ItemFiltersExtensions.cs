using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using NetTopologySuite.Geometries;
using Stac.Api.Services.Queryable;

namespace Stac.Api.Services.Filtering
{
    public static class ItemFiltersExtensions
    {

        public static bool IsLike(object like, IComparable pattern)
        {
            Regex regex = new Regex(@"\A" + new Regex(@"\.|\$|\^|\{|\[|\(|\||\)|\*|\+|\?|\\").Replace(pattern.ToString(), ch => @"\" + ch).Replace('_', '.').Replace("%", ".*") + @"\z", RegexOptions.Singleline);
            return regex.IsMatch(like.ToString());
        }

        public static StacQueryable<TSource> Filter<TSource>(this StacQueryable<TSource> items, double[] bboxArray) where TSource : IStacObject
        {
            if (bboxArray == null)
            {
                return items;
            }

            NetTopologySuite.Geometries.Geometry bboxGeometry = new NetTopologySuite.Geometries.GeometryFactory().ToGeometry(new Envelope(bboxArray[0], bboxArray[2], bboxArray[1], bboxArray[3]));
            return new StacQueryable<TSource>(items.StacQueryProvider, items.Where(i => items.StacQueryProvider.GetStacObjectGeometry<TSource>(i, "geometry").Intersects(bboxGeometry)).Expression);
        }

        public static StacQueryable<TSource> Filter<TSource>(this StacQueryable<TSource> items, DateTime? datetime) where TSource : IStacObject
        {
            if (datetime == null)
                return items;


            return new StacQueryable<TSource>(items.StacQueryProvider, items.Where(i => items.StacQueryProvider.GetStacObjectDateTime<TSource>(i, "datetime")
                    .IntersectsWith(new Itenso.TimePeriod.TimeInterval(datetime.Value, Itenso.TimePeriod.IntervalEdge.Closed, Itenso.TimePeriod.IntervalEdge.Closed, true, false))).Expression);

        }

    }
}