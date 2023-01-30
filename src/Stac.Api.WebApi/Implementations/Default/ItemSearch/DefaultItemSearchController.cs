using System.IO.Abstractions;
using GeoJSON.Net.Geometry;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stac.Api.Clients.ItemSearch;
using Stac.Api.Interfaces;
using Stac.Api.Models;
using Stac.Api.WebApi.Controllers.ItemSearch;
using Stac.Api.WebApi.Implementations.Shared.Geometry;
using Stac.Api.WebApi.Services;
using Stac.Api.WebApi.Services.Context;

namespace Stac.Api.WebApi.Implementations.Default.ItemSearch
{
    public class DefaultItemSearchController : IItemSearchController
    {
        private readonly IStacApiEndpointManager _stacApiEndpointManager;
        private readonly IDataServicesProvider _dataServicesProvider;
        private readonly IStacApiContextFactory _stacApiContextFactory;
        private readonly IStacLinker _stacLinker;

        public DefaultItemSearchController(IStacApiEndpointManager stacApiEndpointManager,
                                            IDataServicesProvider dataServicesProvider,
                                            IStacApiContextFactory stacApiContextFactory,
                                            IStacLinker stacLinker)
        {
            _stacApiEndpointManager = stacApiEndpointManager;
            _dataServicesProvider = dataServicesProvider;
            _stacApiContextFactory = stacApiContextFactory;
            _stacLinker = stacLinker;
        }

        public async Task<ActionResult<StacFeatureCollection>> GetItemSearchAsync(string bbox, IGeometryObject intersects, string datetime, int limit, IEnumerable<string> ids, IEnumerable<string> collections, CancellationToken cancellationToken = default)
        {
            // Create the context
            IStacApiContext stacApiContext = _stacApiContextFactory.Create();

            // Set the collection
            stacApiContext.SetCollections(collections.ToList());

            // Set the Limit
            stacApiContext.Properties.Add(IPaginationParameters.PaginationPropertiesKey, new DefaultPaginationParameters() { Limit = limit });

            IItemsProvider itemsProvider = _dataServicesProvider.GetItemsProvider();

            // Apply Context Pre Query Filters
            _stacApiContextFactory.ApplyContextPreQueryFilters<StacItem>(stacApiContext, itemsProvider);

            // Query the items
            var items = await itemsProvider.GetItemsAsync(stacApiContext, cancellationToken);

            // Prepare filters
            if (ids != null && ids.Any())
            {
                items = items.Where(i => ids.Contains(i.Id));
            }

            if (!string.IsNullOrEmpty(bbox))
            {
                double[] bboxArray = Array.ConvertAll(bbox.Split(','), double.Parse);
                items = items.Where(i => i.Geometry.Intersects(bboxArray));
            }

            if (intersects != null)
            {
                items = items.Where(i => i.Geometry.Intersects(intersects));
            }

            if (!string.IsNullOrEmpty(datetime))
            {
                var datetimeValue = DateTime.Parse(datetime);
                items = items.Where(i => i.DateTime.HasInside(datetimeValue));
            }

            // Save the query parameters in the context
            SetQueryParametersInContext(stacApiContext, bbox, intersects, datetime, limit, ids, collections);

            // Apply Context Post Query Filters
            items = _stacApiContextFactory.ApplyContextPostQueryFilters<StacItem>(stacApiContext, itemsProvider, items);

            StacFeatureCollection fc = new StacFeatureCollection(items);

            // Link the collection
            _stacLinker.Link(fc, stacApiContext);

            // Set the matched count
            if (stacApiContext.Properties.GetProperty<int?>(DefaultConventions.MatchedCountPropertiesKey) != null)
                fc.NumberMatched = stacApiContext.Properties.GetProperty<int?>(DefaultConventions.MatchedCountPropertiesKey).Value;

            return fc;
        }

        private void SetQueryParametersInContext(IStacApiContext stacApiContext, string bbox, IGeometryObject? intersects, string datetime, int? limit, IEnumerable<string>? ids, IEnumerable<string> collections)
        {
            DefaultQueryParameters queryParameters = new DefaultQueryParameters();
            if (bbox != null)
                queryParameters.Add("bbox", bbox);
            if (intersects != null)
                queryParameters.Add("intersects", JsonConvert.SerializeObject(intersects));
            if (datetime != null)
                queryParameters.Add("datetime", datetime);
            if (limit != null)
                queryParameters.Add("limit", limit.ToString());
            if (ids != null)
                queryParameters.Add("ids", string.Join(",", ids));
            if (collections != null)
                queryParameters.Add("collections", string.Join(",", collections));
            
            stacApiContext.Properties.Add(DefaultConventions.QueryParametersPropertiesKey, queryParameters);

        }

        public Task<ActionResult<StacFeatureCollection>> PostItemSearchAsync(SearchBody body, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}