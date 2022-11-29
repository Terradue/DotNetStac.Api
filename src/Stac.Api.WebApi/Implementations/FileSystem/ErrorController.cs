using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Stac.Api.WebApi.Implementations.FileSystem
{
    public class ErrorController : StacApiController
    {
        [Route("/error")]
        public IActionResult HandleError() => Problem();
    }
}