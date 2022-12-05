using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stac.Api.FileSystem.Extensions;
using Stac.Api.Interfaces;
using Stac.Api.WebApi.Controllers.Core;

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
                services.AddFileSystemData(b =>
                    b.UseFileSystemProvider(_datadir, true));
            });
            return base.CreateHost(builder);
        }
    }
}