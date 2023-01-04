using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Stac.Api.Interfaces;
using Stac.Api.Services.Filtering;
using Stac.Api.Services.Queryable;

namespace Stac.Api.Tests
{
    public class StacTestsQueryProvider : StacQueryProvider
    {
        private readonly IEnumerable<IStacObject> _seed;

        public StacTestsQueryProvider(StacQueryablesOptions queryablesOptions, IEnumerable<IStacObject> seed) : base(queryablesOptions)
        {
            _seed = seed;
        }

        public static StacTestsQueryProvider CreateDefaultQueryProvider(HttpContext httpContext, IEnumerable<IStacObject> seed)
        {
            return new StacTestsQueryProvider(StacQueryablesOptions.GenerateDefaultOptions<StacItem>(httpContext), seed);
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
            var treeCopier = new ExpressionTreeModifier(this);
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

            return default(TResult);
        }

        public override Expression GetFirstExpression()
        {
            return _seed.AsQueryable().Expression;
        }
    }
}