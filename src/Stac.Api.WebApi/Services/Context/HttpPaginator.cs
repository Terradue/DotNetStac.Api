using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stac.Api.Interfaces;
using Stac.Api.WebApi.Implementations.Default;

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
            SetLinksInContext(stacApiContext, dataProvider, paginatedItems);
            return paginatedItems;
        }

        private IEnumerable<T> ApplyPagination<T>(IStacApiContext stacApiContext, IDataProvider<T> dataProvider, IEnumerable<T> items) where T : IStacObject
        {
            // If provider is a paginator,
            // we ask the data provider to deal with pagination on the collection the way it wants to
            // otherwise, we use the default pagination
            IPaginator paginator = dataProvider as IPaginator;
            if (paginator == null)
            {
                paginator = this;
            }

            // Get the pagination parameters from the context
            IPaginationParameters paginationParameters = paginator.GetPaginationParameters(stacApiContext);
            // Nothing to do if there are no pagination parameters
            if (paginationParameters == null)
            {
                return items;
            }

            // Apply pagination
            IEnumerable<T> paginatedItems = items;
            if (paginationParameters.Offset.HasValue)
            {
                paginatedItems = paginatedItems.Skip(paginationParameters.Offset.Value);
            }
            if (paginationParameters.Limit.HasValue)
            {
                if (paginationParameters.Page.HasValue)
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
            if (paginationParameters is BodyPaginationParameters)
            {
                nextLinkValues.BodyValues = GetPaginationValues(paginationParameters);
                nextLinkValues.Merge = true;
            }
            else
            {
                IQueryParameters queryParameters = GetQueryParameters(stacApiContext);
                nextLinkValues.QueryValues = GetPaginationValues(paginationParameters, queryParameters);
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
            if (paginationParameters is BodyPaginationParameters)
            {
                previousLinkValues.BodyValues = GetPaginationValues(paginationParameters);
                previousLinkValues.Merge = true;
            }
            else
            {
                IQueryParameters queryParameters = GetQueryParameters(stacApiContext);
                previousLinkValues.QueryValues = GetPaginationValues(paginationParameters, queryParameters);
            }
            return previousLinkValues;
        }

        private IQueryParameters GetQueryParameters(IStacApiContext stacApiContext)
        {
            // Get the query parameters from the context
            IQueryParameters queryParameters = null;
            if (stacApiContext.Properties.ContainsKey(DefaultConventions.QueryParametersPropertiesKey))
            {
                queryParameters = stacApiContext.Properties[DefaultConventions.QueryParametersPropertiesKey] as IQueryParameters;
            }
            return queryParameters;
        }

        private IDictionary<string, object> GetPaginationValues(IPaginationParameters paginationParameters, IQueryParameters queryParameters = null)
        {
            // Transform pagination parameters into a dictionary of values
            IDictionary<string, object> values = new Dictionary<string, object>();
            if (queryParameters != null)
            {
                foreach (var queryParameter in queryParameters)
                {
                    if (!values.ContainsKey(queryParameter.Key))
                    {
                        values.Add(queryParameter.Key, queryParameter.Value);
                    }
                }
            }
            if (paginationParameters.Limit.HasValue && !values.ContainsKey("limit"))
            {
                values.Add("limit", paginationParameters.Limit.Value);
            }
            if (paginationParameters.Offset.HasValue && !values.ContainsKey("offset"))
            {
                values.Add("offset", paginationParameters.Offset.Value);
            }
            if (paginationParameters.Page.HasValue && !values.ContainsKey("page"))
            {
                values.Add("page", paginationParameters.Page.Value);
            }
            if (!string.IsNullOrEmpty(paginationParameters.Token) && !values.ContainsKey("token"))
            {
                values.Add("token", paginationParameters.Token);
            }
            return values;
        }

        public IPaginationParameters GetNextPageParameters<T>(IEnumerable<T> items, IStacApiContext stacApiContext) where T : IStacObject
        {
            // Simply increment the page by 1 if the number of items is equal to the limit
            // otherwise, there is no next page
            DefaultPaginationParameters nextPageParameters = new DefaultPaginationParameters(GetPaginationParameters(stacApiContext));
            if (nextPageParameters == null || items.Count() < nextPageParameters.Limit)
            {
                return null;
            }
            if (nextPageParameters.Page == null)
            {
                nextPageParameters.Page = 1;
            }
            nextPageParameters.Page++;
            return nextPageParameters;
        }

        public IPaginationParameters GetPreviousPageParameters<T>(IEnumerable<T> items, IStacApiContext stacApiContext) where T : IStacObject
        {
            // Simply decrement the page by 1 if the page is greater than 1
            // otherwise, there is no previous page
            DefaultPaginationParameters previousPageParameters = new DefaultPaginationParameters(GetPaginationParameters(stacApiContext));
            if (previousPageParameters == null || !previousPageParameters.Page.HasValue || previousPageParameters.Page <= 1)
            {
                return null;
            }
            previousPageParameters.Page--;
            return previousPageParameters;
        }

        public void PreparePagination(IPaginationParameters paginationParameters, IStacApiContext stacApiContext)
        {
            // Get the existing pagination parameters
            IPaginationParameters existingPaginationParameters = GetPaginationParameters(stacApiContext);
            if (existingPaginationParameters != null)
            {
                if (existingPaginationParameters is BodyPaginationParameters)
                {
                    // If the existing pagination parameters are in the body, we need to merge them with the new ones
                    // The existing ones take precedence
                    BodyPaginationParameters newPaginationParameters = new BodyPaginationParameters(paginationParameters);
                    Merge(newPaginationParameters, existingPaginationParameters);
                    stacApiContext.SetProperty(IPaginationParameters.PaginationPropertiesKey, newPaginationParameters);
                }
                else
                {
                    // If the existing pagination parameters are in the query string, we need to merge them with the new ones
                    // The new ones take precedence
                    QueryStringPaginationParameters newPaginationParameters = new QueryStringPaginationParameters(paginationParameters);
                    Merge(newPaginationParameters, existingPaginationParameters);
                    stacApiContext.SetProperty(IPaginationParameters.PaginationPropertiesKey, newPaginationParameters);
                }
            }
        }

        private void Merge(BodyPaginationParameters newPaginationParameters, IPaginationParameters existingPaginationParameters)
        {
            if (existingPaginationParameters.Limit.HasValue)
            {
                newPaginationParameters.Limit = existingPaginationParameters.Limit;
            }
            if (existingPaginationParameters.Offset.HasValue)
            {
                newPaginationParameters.Offset = existingPaginationParameters.Offset;
            }
            if (existingPaginationParameters.Page.HasValue)
            {
                newPaginationParameters.Page = existingPaginationParameters.Page;
            }
            if (!string.IsNullOrEmpty(existingPaginationParameters.Token))
            {
                newPaginationParameters.Token = existingPaginationParameters.Token;
            }
        }

        private void Merge(QueryStringPaginationParameters newPaginationParameters, IPaginationParameters existingPaginationParameters)
        {
            if (existingPaginationParameters.Limit.HasValue)
            {
                newPaginationParameters.Limit = existingPaginationParameters.Limit;
            }
            if (existingPaginationParameters.Offset.HasValue)
            {
                newPaginationParameters.Offset = existingPaginationParameters.Offset;
            }
            if (existingPaginationParameters.Page.HasValue)
            {
                newPaginationParameters.Page = existingPaginationParameters.Page;
            }
            if (!string.IsNullOrEmpty(existingPaginationParameters.Token))
            {
                newPaginationParameters.Token = existingPaginationParameters.Token;
            }
        }

        public bool CanHandle<T>() where T : IStacObject
        {
            return true;
        }

        public IPaginationParameters GetPaginationParameters(IStacApiContext stacApiContext)
        {
            if (stacApiContext.Properties.ContainsKey(IPaginationParameters.PaginationPropertiesKey))
            {
                return stacApiContext.Properties[IPaginationParameters.PaginationPropertiesKey] as IPaginationParameters;
            }
            return null;
        }
    }
}