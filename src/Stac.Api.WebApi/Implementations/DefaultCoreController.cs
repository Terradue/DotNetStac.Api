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
            var lp = new LandingPage("test", "test");
            lp.ConformanceClasses.Add("https://api.stacspec.org/v1.0.0-rc.1/core");
            return lp;
        }
    }
}