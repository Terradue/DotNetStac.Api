using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stac.Api.Interfaces;

namespace Stac.Api.Extensions.Sort.Context
{
    /// <summary>
    /// HTTP Paginator
    /// This default paginator is used to paginate the results of a query
    /// using the HTTP pagination parameters (limit, page, offset, token)
    /// from the query string or the body of the request
    /// </summary>
    public class SortContextFilter : IStacApiContextFilter, ISortExtension
    {
        public int Priority => 10;

        public void ApplyContextPreQueryFilters<T>(IStacApiContext stacApiContext, IDataProvider<T> dataProvider, IStacApiRequestBody request) where T : IStacObject
        {
            // If provider is a sorter,
            // we ask the data provider to deal with pagination on the collection the way it wants to
            // otherwise, we use the default pagination
            ISortExtension sorter = dataProvider as ISortExtension;
            if (sorter == null)
            {
                sorter = this;
            }

            // By default, we pass the pagination parameters in the query string
            // Those parametrs are susecptible to be overriden by the controller
            // with body sorting parameters if present in the body of the request
            sorter.PrepareSorting(stacApiContext, request);
        }

        public T ApplyContextPostQueryFilters<T>(IStacApiContext stacApiContext, IDataProvider<T> dataProvider, T item) where T : IStacObject
        {
            // Nothing to do here
            return item;
        }

        public IEnumerable<T> ApplyContextPostQueryFilters<T>(IStacApiContext stacApiContext, IDataProvider<T> dataProvider, IEnumerable<T> items) where T : IStacObject
        {
            // If provider is a sorter,
            // we ask the data provider to deal with pagination on the collection the way it wants to
            // otherwise, we use the default pagination
            ISortExtension sorter = dataProvider as ISortExtension;
            if (sorter == null)
            {
                sorter = this;
            }
            IEnumerable<T> sortedItems = sorter.ApplySorting(stacApiContext, dataProvider, items);
            return sortedItems;
        }

        public void ApplyContextResultFilters<T>(IStacApiContext stacApiContext, IDataProvider<T> dataProvider, IStacResultObject<T> result) where T : IStacObject
        {
            // Nothing to do here
        }

        public IEnumerable<T> ApplySorting<T>(IStacApiContext stacApiContext, IDataProvider<T> dataProvider, IEnumerable<T> items) where T : IStacObject
        {
            // Get the sorting parameters from the StacApiContext
            ISortParameters sortParameters = null;
            try
            {
                sortParameters = stacApiContext.GetProperty<ISortParameters>(ISortParameters.SortingPropertiesKey);
            }
            catch (Exception e)
            {
                // Nothing to do here
            }

            if (sortParameters == null)
            {
                return items;
            }
            // For each sort parameter, we sort the items
            foreach (ISortByItem sortByItem in sortParameters)
            {
                items = items.OrderBy(sortByItem.GetKeySelector<T>(items));
            }

            return items;

        }

        /// <summary>
        /// Prepare the sorting by parsing the sort parameter in the query string
        /// of the HTTP request
        /// </summary>
        /// <param name="stacApiContext"></param>
        public ISortParameters FindSortParametersInContext(IStacApiContext stacApiContext)
        {
            // Find the HttpContext in the StacApiContext
            var httpContext = stacApiContext.HttpContext;
            if (httpContext == null)
            {
                return null;
            }
            // Find the query string in the HttpContext
            var queryString = httpContext.Request.Query;
            if (queryString == null)
            {
                return null;
            }
            // Find the sort parameter in the query string
            var sorts = queryString[ISortParameters.QuerySortKeyName];
            if (!sorts.Any())
            {
                return null;
            }
            // Parse the sort parameter
            List<ISortByItem> sortByItems = SortExtensions.ParseSortByParameters(sorts);

            return new DefaultSortBy(sortByItems);
        }

        public void PrepareSorting(IStacApiContext stacApiContext, IStacApiRequestBody request)
        {
            // Find the sort parameters in the request
            ISortParameters sortParametersInRequest = FindSortParametersInContext(stacApiContext);

            // Find the sort parameters in the request body
            ISortParameters sortParametersFromBody = FindSortParametersInRequestBody(request);

            // Merge the sort parameters from the request and the request body
            ISortParameters sortParameters = SortExtensions.MergeParameters(sortParametersInRequest, sortParametersFromBody);

            // Set the sort parameters in the StacApiContext
            if (sortParameters != null && sortParameters.Any())
            {
                stacApiContext.SetProperty(ISortParameters.SortingPropertiesKey, sortParameters);
            }
        }

        private ISortParameters FindSortParametersInRequestBody(IStacApiRequestBody request)
        {
            // Get the sort parameters from the request body
            return request.AdditionalProperties.GetProperty<ISortParameters>(ISortParameters.QuerySortKeyName);
        }

        public bool CanHandle<T>() where T : IStacObject
        {
            // Sort only works on item for now
            return typeof(T) == typeof(StacItem);
        }
    }
}