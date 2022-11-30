using System.IO.Abstractions;
using Microsoft.AspNetCore.Mvc;
using NJsonSchema;
using Stac.Api.Extensions.Transactions;
using Stac.Api.Models;
using Stac.Api.WebApi.Controllers.Collections;
using Stac.Api.WebApi.Controllers.Features;
using Stac.Api.WebApi.Implementations.FileSystem.Collections;
using Stac.Api.WebApi.Implementations.FileSystem.Features;
using Stac.Common;

namespace Stac.Api.WebApi.Implementations.FileSystem.Extensions
{
    public class FileSystemTransactionController : FileSystemBaseController, Controllers.Extensions.Transaction.ITransactionController
    {
        private readonly StacFileSystemTransactionService _stacFileSystemTransactionService;
        private readonly IFeaturesController _fileSystemFeaturesController;
        private readonly ICollectionsController _fileSystemCollectionsController;

        public FileSystemTransactionController(IHttpContextAccessor httpContextAccessor,
                                               StacFileSystemResolver fileSystem,
                                               IFeaturesController fileSystemFeaturesController,
                                               ICollectionsController fileSystemCollectionsController,
                                               LinkGenerator linkGenerator) : base(httpContextAccessor, linkGenerator, fileSystem)
        {
            _stacFileSystemTransactionService = new StacFileSystemTransactionService(fileSystem);
            _fileSystemFeaturesController = fileSystemFeaturesController;
            _fileSystemCollectionsController = fileSystemCollectionsController;
        }

        public Task<IActionResult> DeleteFeatureAsync(string if_Match, string collectionId, string featureId, CancellationToken cancellationToken = default)
        {
            CheckExists(collectionId, featureId);
            CheckEtag(if_Match, collectionId, featureId);
            _stacFileSystemTransactionService.DeleteStacItem(collectionId, featureId);
            return Task.FromResult<IActionResult>(new NoContentResult());
        }

        public async Task<ActionResult<StacItem>> PatchFeatureAsync(string if_Match, Patch body, string collectionId, string featureId, CancellationToken cancellationToken = default)
        {
            CheckExists(collectionId, featureId);
            CheckEtag(if_Match, collectionId, featureId);
            var item = await _fileSystemFeaturesController.GetFeatureAsync(collectionId, featureId);
            var newItem = item.Value.Patch(body);
            return await UpdateFeatureAsync(if_Match, newItem, collectionId, featureId, cancellationToken);
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

        public async Task<ActionResult<IEnumerable<StacItem>>> PostFeaturesAsync(StacFeatureCollection collection, string collectionId, CancellationToken cancellationToken = default)
        {
            foreach (var item in collection.Items)
            {
                try
                {
                    var existingItem = _stacFileSystemReaderService.GetStacItemById(collectionId, item.Id);
                    if (item != null)
                    {
                        return new ConflictResult();
                    }
                }
                catch { }
            }
            List<StacItem> items = new List<StacItem>();
            foreach (var item in collection.Items)
            {
                items.Add(await _stacFileSystemTransactionService.CreateStacItemAsync(item, collectionId, cancellationToken));
            }
            return items;
        }

        public async Task<ActionResult<StacItem>> PostFeatureAsync(StacItem body, string collectionId, CancellationToken cancellationToken = default)
        {
            StacItem item = null;
            try
            {
                item = _stacFileSystemReaderService.GetStacItemById(collectionId, body.Id);
                if (item != null)
                {
                    return new ConflictResult();
                }
            }
            catch { }
            item = await _stacFileSystemTransactionService.CreateStacItemAsync(body, collectionId, cancellationToken);
            item.Links.Add(GetSelfLink(item));
            item.Links.Add(GetRootLink(item));
            item.Links.Add(GetCollectionLink(item));
            return new CreatedResult(
                        GetSelfUrl(item),
                        item);
        }

        public async Task<ActionResult<StacItem>> UpdateFeatureAsync(string if_Match, StacItem body, string collectionId, string featureId, CancellationToken cancellationToken = default)
        {
            CheckExists(collectionId, featureId);
            CheckEtag(if_Match, collectionId, featureId);
            CheckFeatureId(body, featureId);
            body.ValidateInputForTransaction();
            return await _stacFileSystemTransactionService.UpdateStacItemAsync(if_Match, body, collectionId, cancellationToken);
        }


    }
}