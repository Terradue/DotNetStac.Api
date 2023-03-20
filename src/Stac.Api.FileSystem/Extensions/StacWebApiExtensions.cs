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
using Stac.Api.WebApi.Patterns.CollectionBased;
using Stac.Api.WebApi.Implementations.Default.Services;
using Stac.Api.WebApi.Services.Context;
using Stac.Api.Services.Default;
using Stac.Api.Extensions.Sort.Context;
using Stac.Api.Services.Debug;

namespace Stac.Api.FileSystem.Extensions
{
    public static class StacWebApiExtensions
    {

        public static IServiceCollection AddFileSystemData(this IServiceCollection services, Action<IStacWebApiBuilder> configure)
        {
            // Add the file system data services
            services.AddSingleton<IDataServicesProvider, FileSystemDataServicesProvider>();
            // Add the Http Stac Api context factory
            services.AddSingleton<IStacApiContextFactory, HttpStacApiContextFactory>();
            // Add the default context filters provider
            services.AddSingleton<IStacApiContextFiltersProvider, DefaultStacContextFiltersProvider>();
            // Register the HTTP pagination filter
            services.AddSingleton<IStacApiContextFilter, HttpPaginator>();
            // Register the sorting filter
            services.AddSingleton<IStacApiContextFilter, SortContextFilter>();
            // Register the debug filter
            services.AddSingleton<IStacApiContextFilter, DebugContextFilter>();
            // The filesystem implements a collection based pattern
            services.AddSingleton<IStacLinker, CollectionBasedStacLinker>();
            // Add the default controllers
            services.AddDefaultControllers();
            // Add the default extensions
            services.AddDefaultStacApiExtensions();
            // Let's Configure
            var builder = new StacWebApiBuilder(services);
            configure(builder);
            return services;
        }

    }
}