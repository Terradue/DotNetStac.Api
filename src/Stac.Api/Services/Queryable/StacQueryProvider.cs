using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GeoJSON.Net.Geometry;
using Itenso.TimePeriod;
using NetTopologySuite.Geometries;
using Newtonsoft.Json.Schema;
using Stac.Api.Interfaces;
using Stac.Api.Models;

namespace Stac.Api.Services.Queryable
{
    public abstract class StacQueryProvider : IStacQueryProvider
    {
        private StacQueryables _stacItemQueryablesOptions;

        public StacQueryProvider(StacQueryables queryablesOptions)
        {
            StacItemQueryablesOptions = queryablesOptions;
        }

        public StacQueryables StacItemQueryablesOptions { get => _stacItemQueryablesOptions; private set => _stacItemQueryablesOptions = value; }

        #region IQueryProvider Members

        public virtual IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            if (typeof(IStacObject).IsAssignableFrom(typeof(TElement)))
            {
                return new StacQueryable<TElement>(this, expression) as IQueryable<TElement>;
            }
            else
            {
                throw new NotImplementedException($"The type {typeof(TElement)} is not supported by the StacQueryProvider");
            }
        }

        public IQueryable CreateQuery(Expression expression)
        {
            throw new NotImplementedException();
        }

        public abstract TResult Execute<TResult>(Expression expression);

        public object Execute(Expression expression)
        {
            throw new NotImplementedException();
        }

        #endregion

        public virtual StacQueryables GetQueryables()
        {
            return StacItemQueryablesOptions;
        }

        public virtual IComparable GetStacObjectProperty<TSource>(TSource s, string property) where TSource : IStacObject
        {
            IComparable result = null;
            if (s is StacItem stacItem)
            {
                if (property == "id")
                    result = stacItem.Id;
                else
                if (property == "bbox")
                    result = stacItem.BoundingBoxes.ToString();
                else
                if (property == "geometry")
                    result = stacItem.Geometry.ToString();
                else
                if (property == "collection")
                    result = stacItem.Collection;
            }
            else
            {
                result = s.GetProperty<IComparable>(property);
            }
            if (result == null)
            {
                return new NullComparable();
            }
            return result;
        }

        public abstract Geometry GetStacObjectGeometry<TSource>(TSource s, string property = "geometry") where TSource : IStacObject;

        public virtual bool SpatialIntersects(Geometry geometry1, Geometry geometry2)
        {
            return geometry1.Intersects(geometry2);
        }

        public abstract ITimePeriod GetStacObjectDateTime<TSource>(TSource i, string v) where TSource : IStacObject;

        public bool TemporalIntersects(ITimePeriod timePeriod1, ITimePeriod timePeriod2)
        {
            return timePeriod1.HasInside(timePeriod2.Start) || timePeriod1.HasInside(timePeriod2.End) || timePeriod2.HasInside(timePeriod1.Start) || timePeriod2.HasInside(timePeriod1.End);
        }
    }
}