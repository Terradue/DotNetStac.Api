using System.Collections.Generic;

namespace Stac.Api.Interfaces
{
    public interface IStacApiController
    {
        IReadOnlyCollection<string> GetConformanceClasses();

        IReadOnlyCollection<StacLink> GetLandingPageLinks(IStacApiContext stacApiContext);

        object GetActionParameters(string actionName);
    }
}