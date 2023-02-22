using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using Stac.Api.Clients.Extensions.Filter;
using Stac.Api.Converters;
using Stac.Api.Interfaces;
using Stac.Api.Models;
using Stac.Api.Models.Cql2;
using Stac.Api.WebApi.Controllers.Extensions.Filter;
using Stac.Api.WebApi.Services;
using Stac.Api.WebApi.Services.Context;

namespace Stac.Api.WebApi.Implementations.Default.Extensions.Filter
{
    public class DefaultFilterController : IFilterController
    {
        private readonly IStacApiEndpointManager _stacApiEndpointManager;
        private readonly IDataServicesProvider _dataServicesProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStacApiContextFactory _stacApiContextFactory;
        private readonly IStacLinker _stacLinker;
        

        public DefaultFilterController(IStacApiEndpointManager stacApiEndpointManager,
                                            IDataServicesProvider dataServicesProvider,
                                            IHttpContextAccessor httpContextAccessor,
                                            IStacApiContextFactory stacApiContextFactory,
                                            IStacLinker stacLinker)
        {
            _stacApiEndpointManager = stacApiEndpointManager;
            _dataServicesProvider = dataServicesProvider;
            _httpContextAccessor = httpContextAccessor;
            _stacApiContextFactory = stacApiContextFactory;
            _stacLinker = stacLinker;
            
        }

        public async Task<ActionResult<StacFeatureCollection>> GetItemSearchAsync(CQL2Filter filter, FilterLang? filter_lang, Uri filter_crs, CancellationToken cancellationToken = default)
        {
            // Create the context
            IStacApiContext stacApiContext = _stacApiContextFactory.Create();

            IItemsProvider itemsProvider = _dataServicesProvider.GetItemsProvider();

            // Apply Context Pre Query Filters
            _stacApiContextFactory.ApplyContextPreQueryFilters<StacItem>(stacApiContext, itemsProvider);

            // Query the items
            var items = await itemsProvider.GetItemsAsync(stacApiContext, cancellationToken);

            // Apply the filter
            items = items.Boolean<StacItem>(filter.Expression);

            // Log the filter expression
            stacApiContext.Properties.SetProperty(DefaultConventions.DebugExpressionTreePropertiesKey, items.AsQueryable().Expression.ToString());

            // Save the query parameters in the context
            SetQueryParametersInContext(stacApiContext, filter);

            // Apply Context Post Query Filters
            items = _stacApiContextFactory.ApplyContextPostQueryFilters<StacItem>(stacApiContext, itemsProvider, items);

            StacFeatureCollection fc = new StacFeatureCollection(items);

            // Link the collection
            _stacLinker.Link(fc, stacApiContext);

            // Set the matched count
            if (stacApiContext.Properties.GetProperty<int?>(DefaultConventions.MatchedCountPropertiesKey) != null)
                fc.NumberMatched = stacApiContext.Properties.GetProperty<int?>(DefaultConventions.MatchedCountPropertiesKey).Value;
            if (stacApiContext.Properties.GetProperty<string>(DefaultConventions.DebugExpressionTreePropertiesKey) != null)
                fc.AdditionalProperties.Add(DefaultConventions.DebugExpressionTreePropertiesKey, stacApiContext.Properties.GetProperty<string>(DefaultConventions.DebugExpressionTreePropertiesKey));

            return fc;
        }



        public Task<ActionResult<JsonSchema>> GetQueryablesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult<JsonSchema>> GetQueryablesForCollectionAsync(string collectionId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult<StacFeatureCollection>> PostItemSearchAsync(FilterSearchBody body, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        

        private void SetQueryParametersInContext(IStacApiContext stacApiContext, IFilter filter)
        {
            DefaultQueryParameters queryParameters = new DefaultQueryParameters();
            if (filter != null)
                queryParameters.Add("filter", filter);

            stacApiContext.Properties.Add(DefaultConventions.QueryParametersPropertiesKey, queryParameters);

        }
    }
}