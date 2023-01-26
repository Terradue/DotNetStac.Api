using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stac.Api.Interfaces;

namespace Stac.Api.WebApi.Services.Context
{
    /// <summary>
    /// HTTP Paginator
    /// This default paginator is used to paginate the results of a query
    /// using the HTTP pagination parameters (limit, page, offset, token)
    /// from the query string or the body of the request
    /// </summary>
    public class HttpPaginator : IStacApiContextFilter, IPaginator
    {
        public void ApplyContextPreQueryFilters<T>(IStacApiContext stacApiContext, IDataProvider<T> dataProvider) where T : IStacObject
        {
            // If provider is a paginator,
            // we ask the data provider to deal with pagination on the collection the way it wants to
            // otherwise, we use the default pagination
            IPaginator paginator = dataProvider as IPaginator;
            if (paginator == null)
            {
                paginator = this;
            }

            // By default, we pass the pagination parameters in the query string
            // Those parametrs are susecptible to be overriden by the controller
            // with body pagination parameters if present in the body of the request
            paginator.PreparePagination(QueryStringPaginationParameters.GetPaginatorParameters(stacApiContext.HttpContext), stacApiContext);
        }

        public T ApplyContextPostQueryFilters<T>(IStacApiContext stacApiContext, IDataProvider<T> dataProvider, T item) where T : IStacObject
        {
            // Nothing to do here
            return item;
        }

        public IEnumerable<T> ApplyContextPostQueryFilters<T>(IStacApiContext stacApiContext, IDataProvider<T> dataProvider, IEnumerable<T> items) where T : IStacObject
        {
            IEnumerable<T> paginatedItems = ApplyPagination(stacApiContext, dataProvider, items);
            SetLinksInContext(stacApiContext, dataProvider, items);
            return paginatedItems;
        }

        private IEnumerable<T> ApplyPagination<T>(IStacApiContext stacApiContext, IDataProvider<T> dataProvider, IEnumerable<T> items) where T : IStacObject
        {
            // Get the pagination parameters from the context
            IPaginationParameters paginationParameters = stacApiContext.GetProperty<IPaginationParameters>(IPaginationParameters.PaginationPropertiesKey);
            // Nothing to do if there are no pagination parameters
            if (paginationParameters == null)
            {
                return items;
            }

            // Apply pagination
            IEnumerable<T> paginatedItems = items;
            if ( paginationParameters.Offset.HasValue )
            {
                paginatedItems = paginatedItems.Skip(paginationParameters.Offset.Value);
            }
            if ( paginationParameters.Limit.HasValue )
            {
                if ( paginationParameters.Page.HasValue )
                {
                    paginatedItems = paginatedItems.Skip(paginationParameters.Limit.Value * (paginationParameters.Page.Value - 1));
                }
                paginatedItems = paginatedItems.Take(paginationParameters.Limit.Value);
            }
            return paginatedItems;
        }

        private void SetLinksInContext<T>(IStacApiContext stacApiContext, IDataProvider<T> dataProvider, IEnumerable<T> items) where T : IStacObject
        {
            // If provider is a paginator,
            // we ask the data provider to deal with pagination on the collection the way it wants to
            // otherwise, we use the default pagination
            IPaginator paginator = dataProvider as IPaginator;
            if (paginator == null)
            {
                paginator = this;
            }

            // Add the pagination links
            // We add the next link only if there is a next page
            ILinkValues nextLink = GetNextLink<T>(paginator, items, stacApiContext);
            if (nextLink != null)
            {
                stacApiContext.LinkValues.Add(nextLink);
            }
            // We add the previous link only if there is a previous page
            ILinkValues previousLink = GetPreviousLink<T>(paginator, items, stacApiContext);
            if (previousLink != null)
            {
                stacApiContext.LinkValues.Add(previousLink);
            }
        }

        private ILinkValues GetNextLink<T>(IPaginator paginator, IEnumerable<T> items, IStacApiContext stacApiContext) where T : IStacObject
        {
            // Get pagination parameters for next page
            IPaginationParameters paginationParameters = paginator.GetNextPageParameters<T>(items, stacApiContext);
            if (paginationParameters == null)
            {
                return null;
            }
            // build the link
            var nextLinkValues = new LinkValues(
                ILinkValues.LinkRelationType.Next,
                new Microsoft.AspNetCore.Routing.RouteData(stacApiContext.HttpContext.Request.RouteValues));

            nextLinkValues.Method = new HttpMethod(stacApiContext.HttpContext.Request.Method);
            if (!ArePaginationParametersInBody(stacApiContext))
            {
                nextLinkValues.QueryValues = GetPaginationValues(paginationParameters);
            }
            else
            {
                nextLinkValues.BodyValues = GetPaginationValues(paginationParameters);
            }
            return nextLinkValues;
        }

        private ILinkValues GetPreviousLink<T>(IPaginator paginator, IEnumerable<T> items, IStacApiContext stacApiContext) where T : IStacObject
        {
            // Get pagination parameters for previous page
            IPaginationParameters paginationParameters = paginator.GetPreviousPageParameters<T>(items, stacApiContext);
            if (paginationParameters == null)
            {
                return null;
            }
            // build the link
            var previousLinkValues = new LinkValues(
                ILinkValues.LinkRelationType.Previous,
                new Microsoft.AspNetCore.Routing.RouteData(stacApiContext.HttpContext.Request.RouteValues));
            previousLinkValues.Method = new HttpMethod(stacApiContext.HttpContext.Request.Method);
            if (!ArePaginationParametersInBody(stacApiContext))
            {
                previousLinkValues.QueryValues = GetPaginationValues(paginationParameters);
            }
            else
            {
                previousLinkValues.BodyValues = GetPaginationValues(paginationParameters);
            }
            return previousLinkValues;
        }

        private IDictionary<string, object> GetPaginationValues(IPaginationParameters paginationParameters)
        {
            // Transform pagination parameters into a dictionary of values
            IDictionary<string, object> values = new Dictionary<string, object>();
            if (paginationParameters.Limit.HasValue)
            {
                values.Add("limit", paginationParameters.Limit.Value);
            }
            if (paginationParameters.Offset.HasValue)
            {
                values.Add("offset", paginationParameters.Offset.Value);
            }
            if (paginationParameters.Page.HasValue)
            {
                values.Add("page", paginationParameters.Page.Value);
            }
            if (!string.IsNullOrEmpty(paginationParameters.Token))
            {
                values.Add("token", paginationParameters.Token);
            }
            return values;
        }

        private bool ArePaginationParametersInBody(IStacApiContext stacApiContext)
        {
            HttpStacApiContext httpStacApiContext = stacApiContext as HttpStacApiContext;
            if (httpStacApiContext == null)
            {
                return false;
            }
            return httpStacApiContext.GetProperty<IPaginationParameters>(IPaginationParameters.PaginationPropertiesKey) is BodyPaginationParameters;
        }

        public IPaginationParameters GetNextPageParameters<T>(IEnumerable<T> items, IStacApiContext stacApiContext) where T : IStacObject
        {
            // Simply increment the page by 1 if the number of items is equal to the limit
            // otherwise, there is no next page
            DefaultPaginationParameters nextPageParameters = new DefaultPaginationParameters(
                stacApiContext.GetProperty<IPaginationParameters>(IPaginationParameters.PaginationPropertiesKey));
            if (nextPageParameters == null || items.Count() < nextPageParameters.Limit)
            {
                return null;
            }
            nextPageParameters.Page++;
            return nextPageParameters;
        }

        public IPaginationParameters GetPreviousPageParameters<T>(IEnumerable<T> items, IStacApiContext stacApiContext) where T : IStacObject
        {
            // Simply decrement the page by 1 if the page is greater than 1
            // otherwise, there is no previous page
            DefaultPaginationParameters previousPageParameters = new DefaultPaginationParameters(
                stacApiContext.GetProperty<IPaginationParameters>(IPaginationParameters.PaginationPropertiesKey));
            if (previousPageParameters == null || previousPageParameters.Page <= 1)
            {
                return null;
            }
            previousPageParameters.Page--;
            return previousPageParameters;
        }

        public void PreparePagination(IPaginationParameters paginationParameters, IStacApiContext paginationContext)
        {
            paginationContext.SetProperty(IPaginationParameters.PaginationPropertiesKey, paginationParameters);
        }
    }
}