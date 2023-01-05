using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GeoJSON.Net.Geometry;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Stac.Api.Interfaces;
using Stac.Api.Services.Filtering;
using Stac.Api.Services.Queryable;
using Stars.Geometry.NTS;

namespace Stac.Api.WebApi.Implementations.Default.Services
{
    public class DefaultStacQueryProvider : StacQueryProvider
    {
        private readonly IEnumerable<IStacObject> _seed;

        private DefaultStacQueryProvider(StacQueryablesOptions queryablesOptions, IEnumerable<IStacObject> seed) : base(queryablesOptions)
        {
            _seed = seed;
        }

        public static DefaultStacQueryProvider CreateDefaultQueryProvider(HttpContext httpContext, IEnumerable<IStacObject> seed)
        {
            return new DefaultStacQueryProvider(StacQueryablesOptions.GenerateDefaultOptions<StacItem>(httpContext), seed);
        }

        public override TResult Execute<TResult>(Expression expression)
        {
            bool isEnumerable = typeof(TResult).IsGenericType &&
            typeof(TResult).GetGenericTypeDefinition() == typeof(IEnumerable<>);
            Type itemType = isEnumerable
                // TResult is an IEnumerable`1 collection.
                ? typeof(TResult).GetGenericArguments().Single()
                // TResult is not an IEnumerable`1 collection, but a single item.
                : typeof(TResult);
            
            // Copy the expression tree that was passed in, changing only the first
            // argument of the innermost MethodCallExpression.
            var treeCopier = new DefaultExpressionTreeModifier(this);
            var newExpressionTree = treeCopier.Visit(expression);
            

            // This step creates an IQueryable that executes by replacing Queryable methods with Enumerable methods.
            if (isEnumerable)
            {
                return (TResult)_seed.AsQueryable().Provider.CreateQuery(newExpressionTree);
            }
            else
            {
                return (TResult)_seed.AsQueryable().Provider.Execute(newExpressionTree);
            }

        }

        public override Expression GetFirstExpression()
        {
            return _seed.AsQueryable().Expression;
        }

        public override Geometry? GetStacObjectGeometry<TSource>(TSource s, string property = "geometry")
        {
            if ( s is StacItem stacItem)
            {
                if (stacItem.Geometry != null)
                {
                    return stacItem.Geometry.ToNTSGeometry();
                }
            }
            return GetStacObjectProperty<TSource>(s, property) as Geometry;
        }
    }
}