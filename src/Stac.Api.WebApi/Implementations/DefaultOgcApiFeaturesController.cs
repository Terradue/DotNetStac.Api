using Microsoft.AspNetCore.Mvc;
using Stac.Api.Models;
using Stac.Api.WebApi.Controllers;

namespace Stac.Api.WebApi.Implementations
{
    public class DefaultOgcApiFeaturesController : DefaultBaseController, IOgcApiFeaturesController
    {
        public DefaultOgcApiFeaturesController(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
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
            var collection = GetCollections("examples/collections").FirstOrDefault(c => c.Id == collectionId);
            if (collection == null) return new NotFoundResult();

            var item = GetItems("examples/collections", collectionId).FirstOrDefault(i => i.Id == featureId);
            return item == null ? new NotFoundResult() : (ActionResult<StacItem>)item;
        }

        public async Task<ActionResult<StacFeatureCollection>> GetFeaturesAsync(string collectionId, int limit, object bbox, string datetime, CancellationToken cancellationToken = default)
        {
            var collection = GetCollections("examples/collections").FirstOrDefault(c => c.Id == collectionId);
            if (collection == null) return new NotFoundResult();

            var items = GetItems("examples/collections", collectionId);
            StacFeatureCollection fc = new StacFeatureCollection(items);

            return fc;
        }
    }
}