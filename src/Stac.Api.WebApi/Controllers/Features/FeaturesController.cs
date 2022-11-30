using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Stac.Api.Attributes;

namespace Stac.Api.WebApi.Controllers.Features
{
    [EnableCors("All")]
    [ConformanceClass("https://api.stacspec.org/v1.0.0-rc.2/ogcapi-features")]
    [LandingPageAction("GetConformanceClasses", "conformance", "application/json")]
    public partial class FeaturesController: Stac.Api.WebApi.StacApiController
    {
    }
}