using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stac.Api.WebApi.Controllers.Core;
using Stac.Api.WebApi.Implementations;
using Stac.Api.WebApi.Implementations.FileSystem;

namespace Stac.Api.Tests
{
    internal class CoreStacApiApplication : WebApplicationFactory<CoreController>
    {

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<ICoreController, FileSystemCoreController>();
            });
            return base.CreateHost(builder);
        }
    }
}