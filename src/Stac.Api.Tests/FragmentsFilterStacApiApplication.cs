using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stac.Api.WebApi.Controllers.Extensions.Filter;
using Stac.Api.WebApi.Controllers.Fragments.Filter;
using Stac.Api.WebApi.Implementations;
using Stac.Api.WebApi.Implementations.FileSystem;
using Stac.Api.WebApi.Implementations.FileSystem.Extensions;

namespace Stac.Api.Tests
{
    internal class FragmentsFilterStacApiApplication : WebApplicationFactory<FilterController>
    {

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<IFilterController, FileSystemFilterController>();
            });
            return base.CreateHost(builder);
        }
    }
}