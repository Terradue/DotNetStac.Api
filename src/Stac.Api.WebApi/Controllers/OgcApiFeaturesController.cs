using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;

namespace Stac.Api.WebApi.Controllers
{
    [EnableCors("All")]
    public partial class OgcApiFeaturesController: Stac.Api.WebApi.StacApiController
    {
    }
}