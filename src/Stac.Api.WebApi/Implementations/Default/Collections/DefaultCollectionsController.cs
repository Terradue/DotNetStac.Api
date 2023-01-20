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

            ICollectionsProvider collectionsProvider = _dataServicesProvider.GetCollectionsProvider();
            StacCollection collection = await collectionsProvider.GetCollectionByIdAsync(collectionId, stacApiContext, cancellationToken);
            if (collection == null)
            {
                return new NotFoundResult();
            }
            _stacLinker.Link(collection, stacApiContext);
            return collection;
        }


        public async Task<ActionResult<StacCollections>> GetCollectionsAsync(CancellationToken cancellationToken = default)
        {
            // Create the context
            IStacApiContext stacApiContext = _stacApiContextFactory.Create();

            // Get the collections provider
            ICollectionsProvider collectionsProvider = _dataServicesProvider.GetCollectionsProvider();

            // Get collections from the provider
            var collectionsQueryable = await collectionsProvider.GetCollectionsAsync(stacApiContext, cancellationToken);

            // Create the collection container and link them properly
            StacCollections collections = new StacCollections()
            {
                Collections = collectionsQueryable.Select(c =>
                {
                    _stacLinker.Link(c, stacApiContext);
                    return c;
                }).ToList()
            };
            // Link the collections container
            _stacLinker.Link(collections, stacApiContext);

            return collections;
        }
    }
}