using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Stac.Api.Generated.Controllers;

namespace Stac.Api.Tests
{
    internal class CoreStacApiApplication : WebApplicationFactory<CoreController>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            return base.CreateHost(builder);
        }
    }
}