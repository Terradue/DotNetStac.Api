using Microsoft.AspNetCore.Mvc;
using Stac.Api.Clients.Features;
using Stac.Api.Models;
using Stac.Api.WebApi.Controllers.Features;
using Stac.Api.WebApi.Implementations.Shared.Geometry;
using Stac.Api.WebApi.Implementations.Shared.Temporal;
using Stac.Api.WebApi.Services;

namespace Stac.Api.WebApi.Implementations.FileSystem.Features
{
    public class FileSystemFeaturesController : FileSystemBaseController, IFeaturesController
    {
        private readonly ILandingPageBuilder _landingPageBuilder;

        public FileSystemFeaturesController(IHttpContextAccessor httpContextAccessor,
                                                  LinkGenerator linkGenerator,
                                                  ILandingPageBuilder landingPageBuilder,
                                                  StacFileSystemResolver fileSystem) : base(httpContextAccessor, linkGenerator, fileSystem)
        {
            _landingPageBuilder = landingPageBuilder;
        }

        public async Task<ActionResult<ConformanceDeclaration>> GetConformanceDeclarationAsync(CancellationToken cancellationToken = default)
        {
            return new ConformanceDeclaration()
            {
                ConformsTo = new List<string>(){
                    "https://api.stacspec.org/v1.0.0-rc.1/ogcapi-features",
                    "https://api.stacspec.org/v1.0.0-rc.1/core",
                    "https://api.stacspec.org/v1.0.0-rc.1/collections",
                    "http://www.opengis.net/spec/ogcapi-features-1/1.0/conf/core",
                    "http://www.opengis.net/spec/ogcapi-features-1/1.0/conf/geojson",
                    "http://www.opengis.net/spec/ogcapi-features-1/1.0/conf/oas30"
                }
            };
        }

        public async Task<ActionResult<StacItem>> GetFeatureAsync(string collectionId, string featureId, CancellationToken cancellationToken = default)
        {
            var item = _stacFileSystemReaderService.GetStacItemById(collectionId, featureId);
            item.Links.Add(GetSelfLink(item));
            item.Links.Add(GetRootLink(item));
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