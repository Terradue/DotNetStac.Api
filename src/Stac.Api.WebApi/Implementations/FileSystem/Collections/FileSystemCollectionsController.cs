using System.IO.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Stac.Api.WebApi.Controllers.Collections;

namespace Stac.Api.WebApi.Implementations.FileSystem.Collections
{
    public class FileSystemCollectionsController : FileSystemBaseController, ICollectionsController
    {

        public FileSystemCollectionsController(IHttpContextAccessor httpContextAccessor,
                                               LinkGenerator linkGenerator,
                                               StacFileSystemResolver fileSystem) : base(httpContextAccessor, linkGenerator, fileSystem)
        {
        }

        public async Task<ActionResult<StacCollection>> DescribeCollectionAsync(string collectionId, CancellationToken cancellationToken = default)
        {
            StacCollection collection = _stacFileSystemReaderService.GetCollectionById(collectionId);
            return collection;
        }

        public async Task<ActionResult<StacCollections>> GetCollectionsAsync(CancellationToken cancellationToken = default)
        {
            return new StacCollections()
            {
                Collections = _stacFileSystemReaderService.GetCollections().Select(c => {
                    c.Links.Add(GetSelfLink(c));
                    c.Links.Add(GetRootLink(c));
                    return c;
                }).ToList()
            };
        }

        
    }
}