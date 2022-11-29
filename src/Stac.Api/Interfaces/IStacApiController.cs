using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Stac.Api.Interfaces
{
    public interface IStacApiController
    {
        IReadOnlyCollection<string> GetConformanceClasses();
        IReadOnlyCollection<StacLink> GetLandingPageLinks(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor);
    }
}