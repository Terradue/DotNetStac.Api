using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stac.Api.Clients.Collections;
using Stac.Api.Interfaces;
using Stac.Api.WebApi.Controllers.Collections;
using Stac.Api.WebApi.Services;

namespace Stac.Api.WebApi.Implementations.Default.Collections
{
    public class DefaultCollectionsController : ICollectionsController
    {
        private readonly IDataServicesProvider _dataServicesProvider;
        private readonly IStacLinker _stacLinker;
        private readonly IStacApiContextFactory _stacApiContextFactory;

        public DefaultCollectionsController(IDataServicesProvider dataServicesProvider,
                                            IStacLinker stacLinker,
                                            IStacApiContextFactory stacApiContextFactory)
        {
            _dataServicesProvider = dataServicesProvider;
            _stacLinker = stacLinker;
            _stacApiContextFactory = stacApiContextFactory;
        }

        public async Task<ActionResult<StacCollection>> DescribeCollectionAsync(string collectionId, CancellationToken cancellationToken = default)
        {
            // Create the context
            IStacApiContext stacApiContext = _stacApiContextFactory.Create();
            stacApiContext.SetCollection(collectionId);
            
            // Get the collections provider
            ICollectionsProvider collectionsProvider = _dataServicesProvider.GetCollectionsProvider();

            // Apply Context Pre Query Filters
            _stacApiContextFactory.ApplyContextPreQueryFilters<StacCollection>(stacApiContext, collectionsProvider);

            // Get the collection from the provider
            StacCollection collection = await collectionsProvider.GetCollectionByIdAsync(collectionId, stacApiContext, cancellationToken);
            if (collection == null)
            {
                return new NotFoundResult();
            }

            // Apply Context Post Query Filters
            _stacApiContextFactory.ApplyContextPostQueryFilters<StacCollection>(stacApiContext, collectionsProvider, collection);

            // Link
            _stacLinker.Link(collection, stacApiContext);
            
            return collection;
        }


        public async Task<ActionResult<StacCollections>> GetCollectionsAsync(CancellationToken cancellationToken = default)
        {
            // Create the context
            IStacApiContext stacApiContext = _stacApiContextFactory.Create();

            // Get the collections provider
            ICollectionsProvider collectionsProvider = _dataServicesProvider.GetCollectionsProvider();

            // Apply Context Pre Query Filters
            _stacApiContextFactory.ApplyContextPreQueryFilters<StacCollection>(stacApiContext, collectionsProvider);

            // Get collections from the provider
            var collectionsQueryable = await collectionsProvider.GetCollectionsAsync(stacApiContext, cancellationToken);

            // Create the collection container
            StacCollections collections = new StacCollections(collectionsQueryable);

            // Apply Context Post Query Filters
            _stacApiContextFactory.ApplyContextPostQueryFilters<StacCollection>(stacApiContext, collectionsProvider, collections);

            // Link the collections
            _stacLinker.Link(collections, stacApiContext);

            return collections;
        }

        public async Task<ActionResult<StacCollections>> GetCollectionsAsync2(CancellationToken cancellationToken = default)
        {
            // Create the context
            IStacApiContext stacApiContext = _stacApiContextFactory.Create();

            // Get the collections provider
            ICollectionsProvider collectionsProvider = _dataServicesProvider.GetCollectionsProvider();

            // Apply Context Pre Query Filters
            _stacApiContextFactory.ApplyContextPreQueryFilters<StacCollection>(stacApiContext, collectionsProvider);

            // Get collections from the provider
            IEnumerable<StacCollection> collections = await collectionsProvider.GetCollectionsAsync(stacApiContext, cancellationToken);

            // Apply Context Post Query Filters
            _stacApiContextFactory.ApplyContextPostQueryFilters<StacCollection>(stacApiContext, collectionsProvider, collections);

            // Create the collection container
            StacCollections collectionsContainer = new StacCollections(collections);

            // Link the collections
            _stacLinker.Link(collectionsContainer, stacApiContext);

            return collectionsContainer;
        }
    }
}