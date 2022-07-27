using System.Net.Http;
using Microsoft.AspNetCore.Mvc;

namespace Stac.Api
{
    public abstract class StacApiController : ControllerBase
    {

        protected StacApiController()
        {
        }
    }
}