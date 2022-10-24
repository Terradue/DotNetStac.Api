using System.IO.Abstractions;
using Microsoft.AspNetCore.Mvc;
using NJsonSchema;
using Stac.Api.Extensions;
using Stac.Api.Models;
using Stac.Api.WebApi.Controllers.Fragments.Filter;

namespace Stac.Api.WebApi.Implementations.FileSystem.Extensions
{
    public class FileSystemTransactionController : FileSystemBaseController, Controllers.Extensions.Transaction.ITransactionController
    {
        public FileSystemTransactionController(IHttpContextAccessor httpContextAccessor,
                                                   StacFileSystemResolver fileSystem) : base(httpContextAccessor, fileSystem)
        {
        }

        public Task<IActionResult> DeleteFeatureAsync(string if_Match, string collectionId, string featureId, CancellationToken cancellationToken = default)
        {
            CheckExists(collectionId, featureId);
            CheckEtag(if_Match, collectionId, featureId);
            DeleteItem(collectionId, featureId);
            return Task.FromResult<IActionResult>(new NoContentResult());
        }

        

        public Task<IActionResult> GetFeatureAsync(string collectionId, string featureId, CancellationToken cancellationToken = default)
        {
            CheckExists(collectionId, featureId);
            return Task.FromResult<IActionResult>(new OkObjectResult(GetFeatureById(collectionId, featureId)));
        }

        public Task<ActionResult<StacItem>> PatchFeatureAsync(string if_Match, PatchStacItem body, string collectionId, string featureId, CancellationToken cancellationToken = default)
        {
            CheckExists(collectionId, featureId);
            CheckEtag(if_Match, collectionId, featureId);
            var item = GetFeatureById(collectionId, featureId);
            var newItem = item.Patch(body);
            return UpdateFeatureAsync(if_Match, newItem, collectionId, featureId, cancellationToken);
        }

        public Task<ActionResult<StacItem>> PostFeatureAsync(PostStacItemOrCollection body, string collectionId, CancellationToken cancellationToken = default)
        {
            ValidateFeatureCollectionForCreation(body);
            
        }

        public Task<ActionResult<StacItem>> UpdateFeatureAsync(string if_Match, StacItem body, string collectionId, string featureId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}