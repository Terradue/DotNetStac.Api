using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stac.Api.WebApi.Controllers.Fragments.Filter;
using Stac.Api.WebApi.Implementations;
using Stac.Api.WebApi.Implementations.FileSystem;

namespace Stac.Api.Tests
{
    internal class FragmentsFilterStacApiApplication : WebApplicationFactory<FragmentsFilterController>
    {

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<IFragmentsFilterController, FileSystemFragmentsFilterController>();
            });
            return base.CreateHost(builder);
        }
    }
}