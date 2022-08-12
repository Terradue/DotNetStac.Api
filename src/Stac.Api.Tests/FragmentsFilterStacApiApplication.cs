using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stac.Api.WebApi.Controllers.Fragments.Filter;

namespace Stac.Api.Tests
{
    internal class FragmentsFilterStacApiApplication : WebApplicationFactory<FragmentsFilterController>
    {

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<IFragmentsFilterController, DefaultFragmentsFilterController>();
            });
            return base.CreateHost(builder);
        }
    }
}