using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Stac.Api.Attributes;

namespace Stac.Api.WebApi.Controllers.Collections
{
    [EnableCors("All")]
    [ConformanceClass("https://api.stacspec.org/v1.0.0-rc.2/collections")]
    [LandingPageAction("GetCollections", "data", "application/json")]
    public partial class CollectionsController: Stac.Api.WebApi.StacApiController
    {
    }
}