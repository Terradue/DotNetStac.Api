using System.IO.Abstractions;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NJsonSchema;
using Stac.Api.Extensions.Transactions;
using Stac.Api.Interfaces;
using Stac.Api.Models;
using Stac.Api.WebApi.Controllers.Collections;
using Stac.Api.WebApi.Controllers.Extensions.Transaction;
using Stac.Api.WebApi.Controllers.Features;
using Stac.Api.WebApi.Services;
using Stac.Common;

namespace Stac.Api.WebApi.Implementations.Default.Extensions.Transaction
{
    public class DefaultTransactionController : ITransactionController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDataServicesProvider _dataServicesProvider;
        private readonly IFeaturesController _fileSystemFeaturesController;
        private readonly IStacLinker _stacLinker;
        private readonly ICollectionsController _fileSystemCollectionsController;

        public DefaultTransactionController(IHttpContextAccessor httpContextAccessor,
                                            IDataServicesProvider dataServicesProvider,
                                            IFeaturesController fileSystemFeaturesController,
                                            IStacLinker stacLinker,
                                            ICollectionsController fileSystemCollectionsController)
        {
            _httpContextAccessor = httpContextAccessor;
            _dataServicesProvider = dataServicesProvider;
            _fileSystemFeaturesController = fileSystemFeaturesController;
            _stacLinker = stacLinker;
            _fileSystemCollectionsController = fileSystemCollectionsController;
        }

        public async Task<IActionResult> DeleteFeatureAsync(string if_Match, string collectionId, string featureId, CancellationToken cancellationToken = default)
        {
            IItemsProvider itemsProvider = _dataServicesProvider.GetItemsProvider(collectionId, _httpContextAccessor.HttpContext);
            StacItem stacItem = await itemsProvider.GetItemByIdAsync(featureId, cancellationToken);
            if (stacItem == null)
            {
                return new NotFoundResult();
            }
            IItemsBroker itemsBroker = _dataServicesProvider.GetItemsBroker(collectionId, _httpContextAccessor.HttpContext);
            await itemsBroker.DeleteItemAsync(featureId, cancellationToken);
            return new NoContentResult();
        }

        public async Task<ActionResult<StacItem>> PatchFeatureAsync(string if_Match, Patch body, string collectionId, string featureId, CancellationToken cancellationToken = default)
        {
            IItemsProvider itemsProvider = _dataServicesProvider.GetItemsProvider(collectionId, _httpContextAccessor.HttpContext);
            StacItem stacItem = await itemsProvider.GetItemByIdAsync(featureId, cancellationToken);
            if (stacItem == null)
            {
                return new NotFoundResult();
            }
            string etag = itemsProvider.GetItemEtag(featureId);
            if (if_Match != null && if_Match != etag)
            {
                throw new StacApiException("If-Match header does not match current ETag", (int)HttpStatusCode.PreconditionFailed);
            }
            var newItem = stacItem.Patch(body);
            IItemsBroker itemsBroker = _dataServicesProvider.GetItemsBroker(collectionId, _httpContextAccessor.HttpContext);
            return await itemsBroker.UpdateItemAsync(newItem, featureId, cancellationToken);
        }

        public async Task<ActionResult<StacItem>> PostFeatureAsync(PostStacItemOrCollection body, string collectionId, CancellationToken cancellationToken = default)
        {
            body.ValidateInputForTransaction();
            if (!body.IsCollection)
            {
                return await PostFeatureAsync(body.StacItem, collectionId, cancellationToken);
            }
            else
            {
                var task = PostFeaturesAsync(body.StacFeatureCollection, collectionId, cancellationToken);
                if ( body.StacFeatureCollection.Items.Count > 1000 ){
                    return new AcceptedResult();
                }
                return (await task).Value.FirstOrDefault();
            }
        }

        private async Task<ActionResult<StacItem>> PostFeatureAsync(StacItem body, string collectionId, CancellationToken cancellationToken)
        {
            IItemsProvider itemsProvider = _dataServicesProvider.GetItemsProvider(collectionId, _httpContextAccessor.HttpContext);
            StacItem stacItem = await itemsProvider.GetItemByIdAsync(body.Id, cancellationToken);
            if (stacItem != null)
            {
                return new ConflictResult();
            }
            IItemsBroker itemsBroker = _dataServicesProvider.GetItemsBroker(collectionId, _httpContextAccessor.HttpContext);
            StacItem createdItem = await itemsBroker.CreateItemAsync(body, cancellationToken);
            _stacLinker.Link(createdItem, _httpContextAccessor.HttpContext);
            return createdItem;
        }

        public async Task<ActionResult<IEnumerable<StacItem>>> PostFeaturesAsync(StacFeatureCollection collection, string collectionId, CancellationToken cancellationToken = default)
        {
            IItemsProvider itemsProvider = _dataServicesProvider.GetItemsProvider(collectionId, _httpContextAccessor.HttpContext);
            foreach (var item in collection.Items)
            {
                try
                {
                    var existingItem = await itemsProvider.GetItemByIdAsync(item.Id, cancellationToken);
                    if (item != null)
                    {
                        return new ConflictResult();
                    }
                }
                catch { }
            }
            List<StacItem> items = new List<StacItem>();
            IItemsBroker itemsBroker = _dataServicesProvider.GetItemsBroker(collectionId, _httpContextAccessor.HttpContext);
            foreach (var item in collection.Items)
            {
                items.Add(await itemsBroker.CreateItemAsync(item, cancellationToken));
            }
            return items;
        }

        public async Task<ActionResult<StacItem>> UpdateFeatureAsync(string if_Match, StacItem body, string collectionId, string featureId, CancellationToken cancellationToken = default)
        {
            IItemsBroker itemsBroker = _dataServicesProvider.GetItemsBroker(collectionId, _httpContextAccessor.HttpContext);
            return await itemsBroker.UpdateItemAsync(body, featureId, cancellationToken);
        }
    }
}