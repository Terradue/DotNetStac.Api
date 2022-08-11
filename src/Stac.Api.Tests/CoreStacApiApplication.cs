using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stac.Api.WebApi.Controllers.Core;
using Stac.Api.WebApi.Implementations;

namespace Stac.Api.Tests
{
    internal class CoreStacApiApplication : WebApplicationFactory<CoreController>
    {

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<ICoreController, DefaultCoreController>();
            });
            return base.CreateHost(builder);
        }
    }
}