using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stac.Api.Models;
using Stac.Api.WebApi.Controllers;

namespace Stac.Api.WebApi.Implementations
{
    public class DefaultCoreController : ICoreController
    { 

        public async Task<LandingPage> GetLandingPageAsync(CancellationToken cancellationToken = default)
        {
            var lp = new LandingPage("sentinel", "Copernicus Sentinel Imagery");
            lp.Description = "Catalog of Copernicus Sentinel 1 and 2 imagery.";

            lp.ConformanceClasses.Add("https://api.stacspec.org/v1.0.0-rc.1/core");
            lp.Links.Add(StacLink.CreateSelfLink(new Uri("http://data.example.org/"), "application/json"));
            lp.Links.Add(new StacLink(new Uri("http://data.example.org/api"), "service-desc", null, "application/vnd.oai.openapi+json;version=3.0"));
            lp.Links.Add(new StacLink(new Uri("http://data.example.org/api.html"), "service-doc", null, "text/html"));
            lp.Links.Add(StacLink.CreateChildLink(new Uri("http://data.example.org/catalogs/sentinel-1"), "application/json", "Sentinel 1 Catalog"));
            lp.Links.Add(StacLink.CreateChildLink(new Uri("http://data.example.org/catalogs/sentinel-2"), "application/json", "Sentinel 2 Catalog"));
            return lp;
        }
    }
}