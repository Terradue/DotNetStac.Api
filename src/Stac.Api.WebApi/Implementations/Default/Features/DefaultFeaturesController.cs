using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stac.Api.Clients.Features;
using Stac.Api.Interfaces;
using Stac.Api.Models;
using Stac.Api.Services.Pagination;
using Stac.Api.WebApi.Controllers.Features;
using Stac.Api.WebApi.Services;

namespace Stac.Api.WebApi.Implementations.Default.Features
{
    public class FileSystemFeaturesController : IFeaturesController
    {
        private readonly IStacApiEndpointManager _stacApiEndpointManager;
        private readonly IDataServicesProvider dataServicesProvider;
        private readonly ICollectionsProvider _collectionsProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStacFilterBuilder _filterBuilder;
        private readonly IStacLinker _stacLinker;

        public FileSystemFeaturesController(IStacApiEndpointManager stacApiEndpointManager,
                                            IDataServicesProvider dataServicesProvider,
                                            ICollectionsProvider collectionsProvider,
                                            IHttpContextAccessor httpContextAccessor,
                                            IStacFilterBuilder filterBuilder,
                                            IStacLinker stacLinker)
        {
            _stacApiEndpointManager = stacApiEndpointManager;
            this.dataServicesProvider = dataServicesProvider;
            _collectionsProvider = collectionsProvider;
            _httpContextAccessor = httpContextAccessor;
            _filterBuilder = filterBuilder;
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
            IItemsProvider _itemsProvider = dataServicesProvider.GetItemsProvider(collectionId, _httpContextAccessor.HttpContext);
            var item = await _itemsProvider.GetItemByIdAsync(featureId, cancellationToken);
            if (item == null)
                return new NotFoundResult();
            _stacLinker.Link(item, _httpContextAccessor.HttpContext);
            return item;
        }

        public async Task<ActionResult<StacFeatureCollection>> GetFeaturesAsync(string collectionId, int limit, string bbox, string datetime, CancellationToken cancellationToken = default)
        {
            ICollectionsProvider collectionsProvider = dataServicesProvider.GetCollectionsProvider(_httpContextAccessor.HttpContext);
            var collection = collectionsProvider.GetCollectionByIdAsync(collectionId, cancellationToken);
            if (collection == null)
            {
                throw new StacApiException($"Collection {collectionId} not found", (int)HttpStatusCode.NotFound);
            }

            IItemsProvider itemsProvider = dataServicesProvider.GetItemsProvider(collectionId, _httpContextAccessor.HttpContext);
            double[] bboxArray = Array.ConvertAll(bbox.Split(','), double.Parse);

            if (itemsProvider is IPaginator<StacItem> paginator)
            {
                paginator.SetPaging(QueryStringPaginationParameters.GetPaginatorParameters(_httpContextAccessor.HttpContext));
            }

            var items = await itemsProvider.GetItemsAsync(_filterBuilder.CreateFilter(bboxArray, datetime), cancellationToken);

            StacFeatureCollection fc = new StacFeatureCollection(items);

            return fc;
        }


    }
}