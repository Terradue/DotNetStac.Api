using Hellang.Middleware.ProblemDetails;
using Stac.Api.CodeGen;
using Stac.Api.WebApi.Controllers.Collections;
using Stac.Api.WebApi.Controllers.Core;
using Stac.Api.WebApi.Controllers.Extensions.Filter;
using Stac.Api.WebApi.Controllers.Extensions.Transaction;
using Stac.Api.WebApi.Controllers.ItemSearch;
using Stac.Api.WebApi.Controllers.Features;
using Stac.Api.WebApi.Services;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using GeoJSON.Net.Converters;
using Microsoft.Extensions.DependencyInjection;
using Stac.Api.Interfaces;
using Stac.Api.WebApi.Extensions;
using Stac.Api.FileSystem.Services;

namespace Stac.Api.FileSystem.Extensions
{
    public static class StacWebApiExtensions
    {

        public static IServiceCollection AddFileSystemData(this IServiceCollection services, Action<IStacWebApiBuilder> configure)
        {
            services.AddSingleton<IDataServicesProvider, FileSystemDataServicesProvider>();
            // Let's Configure
            var builder = new StacWebApiBuilder(services);
            configure(builder);
            return services;
        }

    }
}