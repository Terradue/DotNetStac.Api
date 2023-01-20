using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stac.Api.Clients.Features;
using Stac.Api.Interfaces;
using Stac.Api.Models;
using Stac.Api.WebApi.Controllers.Features;
using Stac.Api.WebApi.Services;

namespace Stac.Api.WebApi.Implementations.Default.Features
{
    public class DefaultFeaturesController : IFeaturesController, IStacLinkValuesProvider<StacItem>
    {
        private readonly IStacApiEndpointManager _stacApiEndpointManager;
        private readonly IDataServicesProvider dataServicesProvider;
        private readonly IStacApiContextFactory _stacApiContextFactory;
        private readonly IStacLinker _stacLinker;

        public DefaultFeaturesController(IStacApiEndpointManager stacApiEndpointManager,
                                            IDataServicesProvider dataServicesProvider,
                                            IStacApiContextFactory stacApiContextFactory,
                                            IStacLinker stacLinker)
        {
            _stacApiEndpointManager = stacApiEndpointManager;
            this.dataServicesProvider = dataServicesProvider;
            _stacApiContextFactory = stacApiContextFactory;
            _stacLinker = stacLinker;
        }

        public async Task<ActionResult<ConformanceDeclaration>> GetConformanceDeclarationAsync(CancellationToken cancellationToken = default)
        {
            var cc = _stacApiEndpointManager.GetConformanceClasses(true);
            return new ConformanceDeclaration()
            {
                ConformsTo = new List<string>(cc)
            };
        }

        public async Task<ActionResult<StacItem>> GetFeatureAsync(string collectionId, string featureId, CancellationToken cancellationToken = default)
        {
            // Create the context
            IStacApiContext stacApiContext = _stacApiContextFactory.Create();
            IItemsProvider _itemsProvider = dataServicesProvider.GetItemsProvider();
            var item = await _itemsProvider.GetItemByIdAsync(featureId, stacApiContext, cancellationToken);
            if (item == null)
                return new NotFoundResult();
            _stacLinker.Link(item, stacApiContext);
            return item;
        }

        public async Task<ActionResult<StacFeatureCollection>> GetFeaturesAsync(string collectionId, int limit, string bbox, string datetime, CancellationToken cancellationToken = default)
        {
            // Create the context
            IStacApiContext stacApiContext = _stacApiContextFactory.Create();
            stacApiContext.SetCollection(collectionId);
            ICollectionsProvider collectionsProvider = dataServicesProvider.GetCollectionsProvider();

            var collection = collectionsProvider.GetCollectionByIdAsync(collectionId, stacApiContext, cancellationToken);
            if (collection == null)
            {
                throw new StacApiException($"Collection {collectionId} not found", (int)HttpStatusCode.NotFound);
            }

            IItemsProvider itemsProvider = dataServicesProvider.GetItemsProvider();

            double[]? bboxArray = null;
            if (!string.IsNullOrEmpty(bbox))
            {
                bboxArray = Array.ConvertAll(bbox.Split(','), double.Parse);
            }

            DateTime? datetimeValue = null;
            if (!string.IsNullOrEmpty(datetime))
            {
                datetimeValue = DateTime.Parse(datetime);
            }

            var items = await itemsProvider.GetItemsAsync(bboxArray, datetimeValue, stacApiContext, cancellationToken);

            StacFeatureCollection fc = new StacFeatureCollection(items);
            fc.Collection = collectionId;

            _stacLinker.Link(fc, stacApiContext);

            return fc;
        }

        public IEnumerable<ILinkValues> GetLinkValues()
        {
            throw new NotImplementedException();
        }
    }
}