using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stac.Api.WebApi.Controllers.Core;
using Stac.Api.WebApi.Implementations;
using Stac.Api.WebApi.Implementations.FileSystem;
using Stac.Api.WebApi.Implementations.FileSystem.Core;

namespace Stac.Api.Tests
{
    internal class StacApiApplication : WebApplicationFactory<CoreController>
    {
        private string _datadir;

        public StacApiApplication(string datadir)
        {
            _datadir = datadir;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Notice there is no `--` prefix in "config"
            builder.UseSetting("catalogRootPath", _datadir);
        }

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