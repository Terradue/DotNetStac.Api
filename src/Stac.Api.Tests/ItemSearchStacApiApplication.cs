using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using Stac.Api.Models;
using Stac.Api.WebApi.Controllers;
using Stac.Api.WebApi.Controllers.ItemSearch;
using Stac.Api.WebApi.Implementations;
using Stac.Api.WebApi.Implementations.FileSystem;

namespace Stac.Api.Tests
{
    internal class ItemSearchStacApiApplication : WebApplicationFactory<ItemSearchController>
    {

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<IItemSearchController, FileSystemItemSearchController>();
            });
            return base.CreateHost(builder);
        }
    }
}