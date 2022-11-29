using Stac.Api.Models;

namespace Stac.Api.WebApi.Services
{
    public interface ILandingPageBuilder
    {
        LandingPage Build(StacCatalog rootCatalog, LinkGenerator linkGenerator);

        LandingPage AddConformanceClasses(LandingPage landingPage);
    }
}