using System.IO.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Stac.Api.Models;
using Stac.Api.WebApi.Controllers.OgcApiFeatures;

namespace Stac.Api.WebApi.Implementations.FileSystem
{
    public class FileSystemOgcApiFeaturesController : FileSystemBaseController, IOgcApiFeaturesController
    {
        public FileSystemOgcApiFeaturesController(IHttpContextAccessor httpContextAccessor,
                                                  StacFileSystemResolver fileSystem) : base(httpContextAccessor, fileSystem)
        {
        }

        public async Task<ActionResult<ConformanceClasses>> GetConformanceDeclarationAsync(CancellationToken cancellationToken = default)
        {
            return new ConformanceClasses()
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
            var collection = GetCollections().FirstOrDefault(c => c.Id == collectionId);
            if (collection == null) return new NotFoundResult();

            // var item = GetItems("examples/collections", collectionId).FirstOrDefault(i => i.Id == featureId);
            // return item == null ? new NotFoundResult() : (ActionResult<StacItem>)item;

            return null;
        }

        public async Task<ActionResult<StacFeatureCollection>> GetFeaturesAsync(string collectionId, int limit, string bbox, string datetime, CancellationToken cancellationToken = default)
        {
            var collection = GetCollections().FirstOrDefault(c => c.Id == collectionId);
            if (collection == null) return new NotFoundResult();

            var items = new List<StacItem>();
            StacFeatureCollection fc = new StacFeatureCollection(items);

            return fc;
        }
    }
}