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
        private readonly IStacApiContextFactory _stacApiContextFactory;
        private readonly IDataServicesProvider _dataServicesProvider;
        private readonly IFeaturesController _fileSystemFeaturesController;
        private readonly IStacLinker _stacLinker;
        private readonly ICollectionsController _fileSystemCollectionsController;

        public DefaultTransactionController(IStacApiContextFactory stacApiContextFactory,
                                            IDataServicesProvider dataServicesProvider,
                                            IFeaturesController fileSystemFeaturesController,
                                            IStacLinker stacLinker,
                                            ICollectionsController fileSystemCollectionsController)
        {
            _stacApiContextFactory = stacApiContextFactory;
            _dataServicesProvider = dataServicesProvider;
            _fileSystemFeaturesController = fileSystemFeaturesController;
            _stacLinker = stacLinker;
            _fileSystemCollectionsController = fileSystemCollectionsController;
        }

        public async Task<IActionResult> DeleteFeatureAsync(string if_Match, string collectionId, string featureId, CancellationToken cancellationToken = default)
        {
            // Create a new context for this request
            IStacApiContext stacApiContext = _stacApiContextFactory.Create();

            IItemsProvider itemsProvider = _dataServicesProvider.GetItemsProvider();
            StacItem stacItem = await itemsProvider.GetItemByIdAsync(featureId, stacApiContext, cancellationToken);
            if (stacItem == null)
            {
                return new NotFoundResult();
            }
            IItemsBroker itemsBroker = _dataServicesProvider.GetItemsBroker();
            await itemsBroker.DeleteItemAsync(featureId, stacApiContext, cancellationToken);
            return new NoContentResult();
        }

        public async Task<ActionResult<StacItem>> PatchFeatureAsync(string if_Match, Patch body, string collectionId, string featureId, CancellationToken cancellationToken = default)
        {
            // Create a new context for this request
            IStacApiContext stacApiContext = _stacApiContextFactory.Create();

            IItemsProvider itemsProvider = _dataServicesProvider.GetItemsProvider();
            StacItem stacItem = await itemsProvider.GetItemByIdAsync(featureId, stacApiContext, cancellationToken);
            if (stacItem == null)
            {
                return new NotFoundResult();
            }
            string etag = itemsProvider.GetItemEtag(featureId, stacApiContext);
            if (if_Match != null && if_Match != etag)
            {
                throw new StacApiException("If-Match header does not match current ETag", (int)HttpStatusCode.PreconditionFailed);
            }
            var newItem = stacItem.Patch(body);
            IItemsBroker itemsBroker = _dataServicesProvider.GetItemsBroker();
            return await itemsBroker.UpdateItemAsync(newItem, featureId, stacApiContext, cancellationToken);
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
                if (!task.Wait(500))
                {
                    return new AcceptedResult();
                }
                return task.Result;
            }
        }

        private async Task<ActionResult<StacItem>> PostFeatureAsync(StacItem body, string collectionId, CancellationToken cancellationToken)
        {
            // Create a new context for this request
            IStacApiContext stacApiContext = _stacApiContextFactory.Create();

            // Set the collection id
            stacApiContext.SetCollections(new List<string>() { collectionId });

            IItemsProvider itemsProvider = _dataServicesProvider.GetItemsProvider();
            StacItem existingItem = await itemsProvider.GetItemByIdAsync(body.Id, stacApiContext, cancellationToken);
            if (existingItem != null)
            {
                return new ConflictResult();
            }
            IItemsBroker itemsBroker = _dataServicesProvider.GetItemsBroker();
            StacItem createdItem = await itemsBroker.CreateItemAsync(body, stacApiContext, cancellationToken);
            _stacLinker.Link(createdItem, stacApiContext);
            return new CreatedResult(createdItem.Links.FirstOrDefault(i => i.RelationshipType == "self").Uri, createdItem);
        }

        private async Task<ActionResult<StacItem>> PostFeaturesAsync(StacFeatureCollection collection, string collectionId, CancellationToken cancellationToken = default)
        {
            // Create a new context for this request
            IStacApiContext stacApiContext = _stacApiContextFactory.Create();
            // Set the collection id
            stacApiContext.SetCollections(new List<string>() { collectionId });
            // Clear the pagination parameters so that we get all items
            // stacApiContext.SetPaginationParameters(null);

            // Check if any of the items already exist
            IItemsProvider itemsProvider = _dataServicesProvider.GetItemsProvider();
            if (itemsProvider.AnyItemsExist(collection.Items, stacApiContext))
            {
                return new ConflictResult();
            }

            // Create the items
            List<StacItem> items = new List<StacItem>();
            IItemsBroker itemsBroker = _dataServicesProvider.GetItemsBroker();
            foreach (var item in collection.Items)
            {
                items.Add(await itemsBroker.CreateItemAsync(item, stacApiContext, cancellationToken));
            }

            // Update the collection in the background
            var collections = await itemsBroker.RefreshStacCollectionsAsync(stacApiContext, cancellationToken).ConfigureAwait(false);

            return new CreatedResult(_stacLinker.GetSelfLink(collections.First(), stacApiContext).Uri, items.FirstOrDefault());
        }

        public async Task<ActionResult<StacItem>> UpdateFeatureAsync(string if_Match, StacItem body, string collectionId, string featureId, CancellationToken cancellationToken = default)
        {
            // Create a new context for this request
            IStacApiContext stacApiContext = _stacApiContextFactory.Create();

            IItemsBroker itemsBroker = _dataServicesProvider.GetItemsBroker();
            var update = await itemsBroker.UpdateItemAsync(body, featureId, stacApiContext, cancellationToken);

            // Clear the pagination parameters so that we get all items
            // stacApiContext.SetPaginationParameters(null);

            // Update the collection in the background
            itemsBroker.RefreshStacCollectionsAsync(stacApiContext, cancellationToken).ConfigureAwait(false);

            return update;
        }
    }
}