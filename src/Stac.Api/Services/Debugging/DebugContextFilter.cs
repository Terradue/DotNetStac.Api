using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Stac.Api.Interfaces;

namespace Stac.Api.Services.Debugging
{
    public class DebugContextFilter : IStacApiContextFilter
    {
        private readonly ILogger<DebugContextFilter> _logger;

        public DebugContextFilter(ILogger<DebugContextFilter> logger)
        {
            _logger = logger;
        }

        public int Priority => 1000;

        public void ApplyContextPreQueryFilters<T>(IStacApiContext stacApiContext, IDataProvider<T> dataProvider, IStacApiRequestBody request) where T : IStacObject
        {
            _logger.LogDebug("[{0}] Request {1}", stacApiContext.Id, request);
        }

        public T ApplyContextPostQueryFilters<T>(IStacApiContext stacApiContext, IDataProvider<T> dataProvider, T item) where T : IStacObject
        {
            _logger.LogDebug("[{0}] PostQuery {1}", stacApiContext.Id, item);
            return item;
        }

        public IEnumerable<T> ApplyContextPostQueryFilters<T>(IStacApiContext stacApiContext, IDataProvider<T> dataProvider, IEnumerable<T> items) where T : IStacObject
        {
            // if items is a queryable, log the expression
            if (items is IQueryable<T> queryable)
            {
                _logger.LogDebug("[{0}] PostQuery {1}", stacApiContext.Id, queryable.Expression);
            }
            return items;
        }

        public void ApplyContextResultFilters<T>(IStacApiContext stacApiContext, IDataProvider<T> dataProvider, IStacResultObject<T> result) where T : IStacObject
        {
            _logger.LogDebug("[{0}] Result {1}", stacApiContext.Id, result);
        }

        public bool CanHandle<T>() where T : IStacObject
        {
            return true;
        }
    }
}