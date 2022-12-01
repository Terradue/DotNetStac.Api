using Microsoft.AspNetCore.Mvc;
using Stac.Api.Clients.Collections;
using Stac.Api.Clients.Features;
using Stac.Api.Models;
using Stac.Api.WebApi.Controllers.Features;
using Stac.Api.WebApi.Implementations.Shared.Geometry;
using Stac.Api.WebApi.Implementations.Shared.Temporal;
using Stac.Api.WebApi.Services;

namespace Stac.Api.WebApi.Implementations.FileSystem.Collections
{
    public class FileSystemFeaturesController : FileSystemBaseController, IFeaturesController
    {
        private readonly IStacApiEndpointManager _stacApiEndpointManager;

        public FileSystemFeaturesController(IHttpContextAccessor httpContextAccessor,
                                               LinkGenerator linkGenerator,
                                               IStacApiEndpointManager stacApiEndpointManager,
                                               StacFileSystemResolver fileSystem) : base(httpContextAccessor, linkGenerator, fileSystem)
        {
            _stacApiEndpointManager = stacApiEndpointManager;
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
            CheckExists(collectionId, featureId);
            var item = _stacFileSystemReaderService.GetStacItemById(collectionId, featureId);
            item.Links.Add(GetSelfLink(item));
            item.Links.Add(GetRootLink());
            item.Links.Add(GetCollectionLink(item));
            return new OkObjectResult(item);
        }

        public async Task<ActionResult<StacFeatureCollection>> GetFeaturesAsync(string collectionId, int limit, string bbox, string datetime, CancellationToken cancellationToken = default)
        {
            var collection = _stacFileSystemReaderService.GetCollectionById(collectionId);

            double[] bboxArray = Array.ConvertAll(bbox.Split(','), double.Parse);

            var items = _stacFileSystemReaderService.GetStacItemsByCollectionId(collectionId)
                                                    .Where(i => i.Geometry.Intersects(bboxArray))
                                                    .Where(i => i.DateTime.Intersects(datetime))
                                                    .Take(limit);

            StacFeatureCollection fc = new StacFeatureCollection(items);

            return fc;
        }


    }
}