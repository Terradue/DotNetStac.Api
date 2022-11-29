using Stac.Api.Attributes;
using Stac.Api.Models;

namespace Stac.Api.WebApi.Controllers.Core
{
    [ConformanceClass("https://api.stacspec.org/v1.0.0-rc.1/core")]
    [ConformanceClass("https://api.stacspec.org/v1.0.0-rc.1/browseable")]
    public partial class CoreController : Stac.Api.WebApi.StacApiController
    {
        
    }
}