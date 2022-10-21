using System.IO.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Stac.Api.WebApi.Controllers.Collections;

namespace Stac.Api.WebApi.Implementations.FileSystem.Collections
{
    public class FileSystemCollectionsController : FileSystemBaseController, ICollectionsController
    {
        public FileSystemCollectionsController(IHttpContextAccessor httpContextAccessor,
                                               StacFileSystemResolver fileSystem) : base(httpContextAccessor, fileSystem)
        {
        }

        public async Task<ActionResult<StacCollection>> DescribeCollectionAsync(string collectionId, CancellationToken cancellationToken = default)
        {
            StacCollection collection = GetCollectionById(collectionId);
            return collection == null ? new NotFoundResult() : (ActionResult<StacCollection>)collection;
        }

        public async Task<ActionResult<StacCollections>> GetCollectionsAsync(CancellationToken cancellationToken = default)
        {
            return new StacCollections()
            {
                Collections = GetCollections().ToList()
            };
        }
        
    }
}