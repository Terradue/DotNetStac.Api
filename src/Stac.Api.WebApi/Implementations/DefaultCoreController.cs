using Microsoft.AspNetCore.Mvc;
using Stac.Api.Models;
using Stac.Api.WebApi.Controllers.Core;

namespace Stac.Api.WebApi.Implementations
{
    public class DefaultCoreController : DefaultBaseController, ICoreController
    {
        public DefaultCoreController(IHttpContextAccessor httpContextAccessor): base (httpContextAccessor)
        {
        }

        public async Task<ActionResult<LandingPage>> GetLandingPageAsync(CancellationToken cancellationToken = default)
        {
            var lp = new LandingPage("sentinel", "Copernicus Sentinel Imagery");
            lp.Description = "Catalog of Copernicus Sentinel 1 and 2 imagery.";

            lp.ConformanceClasses.Add("https://api.stacspec.org/v1.0.0-rc.1/core");
            lp.Links.Add(StacLink.CreateSelfLink(AppBaseUrl, "application/json"));
            lp.Links.Add(new StacLink(new Uri(AppBaseUrl, "/swagger/v1/swagger.json"), "service-desc", null, "application/vnd.oai.openapi+json;version=3.0"));
            lp.Links.Add(new StacLink(new Uri(AppBaseUrl, "/swagger"), "service-doc", null, "text/html"));
            lp.Links.Add(StacLink.CreateChildLink(new Uri(AppBaseUrl, "/catalogs/sentinel-1"), "application/json", "Sentinel 1 Catalog"));
            lp.Links.Add(StacLink.CreateChildLink(new Uri(AppBaseUrl, "/catalogs/sentinel-2"), "application/json", "Sentinel 2 Catalog"));
            return lp;
        }
    }
}