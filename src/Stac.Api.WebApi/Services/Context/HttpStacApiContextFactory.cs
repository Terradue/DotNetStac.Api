using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Stac.Api.Interfaces;

namespace Stac.Api.WebApi.Services.Context
{
    public class HttpStacApiContextFactory : IStacApiContextFactory
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LinkGenerator _linkGenerator;

        public HttpStacApiContextFactory(IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkGenerator = linkGenerator;
        }

        public IStacApiContext Create()
        {
            HttpStacApiContext httpStacApiContext = HttpStacApiContext.Create(_httpContextAccessor.HttpContext);
            return httpStacApiContext;
        }
    }
}