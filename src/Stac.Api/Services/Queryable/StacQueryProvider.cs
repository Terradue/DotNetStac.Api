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

namespace Stac.Api.Services.Queryable
{
    public abstract class StacQueryProvider : IStacQueryProvider
    {
        private StacQueryablesOptions _stacItemQueryablesOptions;

        public StacQueryProvider(StacQueryablesOptions queryablesOptions)
        {
            StacItemQueryablesOptions = queryablesOptions;
        }

        public StacQueryablesOptions StacItemQueryablesOptions { get => _stacItemQueryablesOptions; private set => _stacItemQueryablesOptions = value; }

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

        public abstract Expression GetFirstExpression();

        #endregion

        public virtual JSchema GetQueryables()
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
    }
}