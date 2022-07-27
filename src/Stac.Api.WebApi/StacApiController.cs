using System.Net.Http;
using Microsoft.AspNetCore.Mvc;

namespace Stac.Api.WebApi
{
    public abstract class StacApiController : ControllerBase
    {

        protected StacApiController()
        {
        }
    }
}