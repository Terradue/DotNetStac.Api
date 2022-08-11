using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stac.Api.WebApi.Controllers.Collections;
using Stac.Api.WebApi.Implementations;

namespace Stac.Api.Tests
{
    internal class CollectionsStacApiApplication : WebApplicationFactory<CollectionsController>
    {

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<ICollectionsController, DefaultCollectionsController>();
            });
            return base.CreateHost(builder);
        }
    }
}