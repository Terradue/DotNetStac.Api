using System.IO.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Stac.Api.Models;
using Stac.Api.WebApi.Controllers.Core;
using Stac.Api.WebApi.Services;

namespace Stac.Api.WebApi.Implementations.Default.Core
{
    public class DefaultCoreController : ICoreController
    {
        private readonly ILandingPageProvider _landingPageBuilder;

        public DefaultCoreController(IHttpContextAccessor httpContextAccessor,
                                             ILandingPageProvider landingPageBuilder,
                                             LinkGenerator linkGenerator)
        {
            _landingPageBuilder = landingPageBuilder;
        }

        public async Task<ActionResult<LandingPage>> GetLandingPageAsync(CancellationToken cancellationToken = default)
        {
            return await _landingPageBuilder.GetLandingPageAsync(cancellationToken);
        }
    }
}