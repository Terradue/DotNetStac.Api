using System.IO.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Stac.Api.Models;
using Stac.Api.WebApi.Controllers.Core;
using Stac.Api.WebApi.Services;

namespace Stac.Api.WebApi.Implementations.FileSystem.Core
{
    public class FileSystemCoreController : FileSystemBaseController, ICoreController
    {
        private readonly ILandingPageBuilder _landingPageBuilder;

        public FileSystemCoreController(IHttpContextAccessor httpContextAccessor,
                                        ILandingPageBuilder landingPageBuilder,
                                        LinkGenerator linkGenerator,
                                        StacFileSystemResolver fileSystem) : base(httpContextAccessor, linkGenerator, fileSystem)
        {
            _landingPageBuilder = landingPageBuilder;
        }

        public async Task<ActionResult<LandingPage>> GetLandingPageAsync(CancellationToken cancellationToken = default)
        {
            StacCatalog rootCatalog = new StacCatalog("root", "Root Catalog");

            // Try to load exisitng root catalog
            try
            {
                rootCatalog = await GetRootCatalogAsync(cancellationToken);
            }
            catch (Exception e)
            {
                rootCatalog.Properties.Add("error", e.Message);
            }

            LandingPage landingPage = _landingPageBuilder.Build(rootCatalog, LinkGenerator);

            // lp.ConformanceClasses.Add("https://api.stacspec.org/v1.0.0-rc.1/core");
            // lp.Links.Add(StacLink.CreateSelfLink(AppBaseUrl, "application/json"));
            // lp.Links.Add(StacLink.CreateRootLink(AppBaseUrl, "application/json"));
            // lp.Links.Add(new StacLink(new Uri(AppBaseUrl, "/swagger/v1/swagger.json"), "service-desc", null, "application/vnd.oai.openapi+json;version=3.0"));
            // lp.Links.Add(new StacLink(new Uri(AppBaseUrl, "/swagger"), "service-doc", null, "text/html"));
            // lp.Links.Add(new StacLink(new Uri(AppBaseUrl, "/collections"), "data", null, "application/json"));

            return landingPage;
        }
    }
}