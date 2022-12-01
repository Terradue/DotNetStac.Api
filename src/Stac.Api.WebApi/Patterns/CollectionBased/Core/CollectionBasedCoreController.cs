using System.IO.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Stac.Api.Models;
using Stac.Api.WebApi.Controllers.Core;
using Stac.Api.WebApi.Services;

namespace Stac.Api.WebApi.Implementations.CollectionBased.Core
{
    public class CollectionBasedCoreController : ICoreController
    {
        private readonly ILandingPageProvider _landingPageBuilder;

        public CollectionBasedCoreController(IHttpContextAccessor httpContextAccessor,
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