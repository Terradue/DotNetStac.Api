using Stac.Api.Interfaces;
using Stac.Api.Models;

namespace Stac.Api.WebApi.Services
{
    public interface IStacApiEndpointManager
    {
        IReadOnlyCollection<string> GetConformanceClasses(bool OgcApiOnly = false);

        IEnumerable<IStacApiController> GetRegisteredStacApiControllers();
    }
}