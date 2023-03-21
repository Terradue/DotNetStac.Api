using MartinCostello.Logging.XUnit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stac.Api.FileSystem.Extensions;
using Stac.Api.FileSystem.Services;
using Stac.Api.Interfaces;
using Stac.Api.WebApi.Controllers.Core;
using Xunit.Abstractions;
using Microsoft.Extensions.Logging;

namespace Stac.Api.Tests
{
    public class StacApiApplication : WebApplicationFactory<FileSystemDataServicesProvider>, ITestOutputHelperAccessor
    {
        private string _datadir;

        public StacApiApplication(string datadir, ITestOutputHelper outputHelper) : base()
        {
            _datadir = datadir;
            OutputHelper = outputHelper;
        }

        public ITestOutputHelper OutputHelper { get; set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Notice there is no `--` prefix in "config"
            builder.UseSetting("CatalogRootPath", _datadir);
            builder.ConfigureLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders().AddXUnit(this, c => c.Filter = (s, l) => l >= LogLevel.Debug);
            });
            builder.ConfigureServices(services =>
            {
                services.AddLogging(loggingBuilder =>
                {
                    loggingBuilder.ClearProviders().AddXUnit(this, c => c.Filter = (s, l) => l >= LogLevel.Debug);
                });
            });
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            
            return base.CreateHost(builder);
        }
    }
}