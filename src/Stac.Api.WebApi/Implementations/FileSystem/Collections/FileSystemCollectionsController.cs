using System.IO.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Stac.Api.Clients.Collections;
using Stac.Api.Models;
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
            try
            {
                StacCollection collection = _stacFileSystemReaderService.GetCollectionById(collectionId);
                collection.Links.Add(GetSelfLink(collection));
                collection.Links.Add(GetRootLink());
                return collection;
            }
            catch (IOException)
            {
                return new NotFoundResult();
            }
        }

        

        public async Task<ActionResult<StacCollections>> GetCollectionsAsync(CancellationToken cancellationToken = default)
        {
            var collections = new StacCollections()
            {
                Collections = _stacFileSystemReaderService.GetCollections().Select(c =>
                {
                    c.Links.Add(GetSelfLink(c));
                    c.Links.Add(GetRootLink());
                    return c;
                }).ToList()
            };
            collections.Links.Add(GetSelfLink(collections));
            collections.Links.Add(GetRootLink());
            return collections;
        }

        protected StacApiLink GetSelfLink(StacCollections stacCollections)
        {
            return new StacApiLink(
                new Uri(LinkGenerator.GetUriByAction(HttpContextAccessor.HttpContext, "GetCollections", "Collections", new { })),
                "self",
                "Collections",
                "application/json");
        }


    }
}