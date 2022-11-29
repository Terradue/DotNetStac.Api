using Stac.Api.Models;

namespace Stac.Api.WebApi.Services
{
    public interface ILandingPageBuilder
    {
        LandingPage Build(StacCatalog rootCatalog);

        LandingPage AddConformanceClasses(LandingPage landingPage);
    }
}