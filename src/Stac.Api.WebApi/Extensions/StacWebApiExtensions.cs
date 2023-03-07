using Hellang.Middleware.ProblemDetails;
using Stac.Api.CodeGen;
using Stac.Api.WebApi.Services;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using GeoJSON.Net.Converters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Stac.Api.WebApi.Controllers.Core;
using Stac.Api.WebApi.Implementations.Default.Core;
using Stac.Api.WebApi.Implementations.Default.Collections;
using Stac.Api.WebApi.Controllers.Collections;
using Stac.Api.WebApi.Controllers.ItemSearch;
using Stac.Api.WebApi.Implementations.Default.ItemSearch;
using Stac.Api.WebApi.Controllers.Features;
using Stac.Api.WebApi.Implementations.Default.Features;
using Stac.Api.WebApi.Controllers.Extensions.Filter;
using Stac.Api.WebApi.Controllers.Extensions.Transaction;
using Stac.Api.WebApi.Implementations.Default.Extensions.Transaction;
using GeoJSON.Net.Geometry;
using Stac.Api.Converters;
using Stac.Api.Models.Core;
using Stac.Api.WebApi.Implementations.Default.Extensions.Filter;
using Stac.Api.Clients.Extensions.Filter;
using Stac.Api.WebApi.Filters;
using Stac.Api.WebApi.ModelBinding;
using Stac.Api.WebApi.ApplicationModels;
using Stac.Api.WebApi.ModelBinding.Extensions;

namespace Stac.Api.WebApi.Extensions
{
    public static class StacWebApiExtensions
    {
        public static IServiceCollection AddStacWebApi(this IServiceCollection services)
        {
            services.AddControllers(options =>
                {
                    // Json error handling
                    options.Filters.Add<JsonErrorActionFilter>();
                    // Add the STAC API model binder provider to the controller
                    options.ModelBinderProviders.Insert(0, new StacModelBinderProvider());
                    // Add the STAC API filter extension model binder provider to the controller
                    options.ModelBinderProviders.Insert(1, new FilterExtensionModelBinderProvider());
                    // Add the STAC API extensions convention to the controller
                    options.Conventions.Add(new StacApiExtensionsConvention());
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver.ResolveContract(typeof(IGeometryObject)).Converter = new GeometryConverter();
                    options.SerializerSettings.ContractResolver.ResolveContract(typeof(IntersectGeometryFilter)).Converter = new GeometryFilterConverter<IntersectGeometryFilter>();
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    options.SerializerSettings.Converters.Add(new GeometryConverter());
                    options.SerializerSettings.Converters.Add(new GeometryFilterConverter<IntersectGeometryFilter>());
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.AllowInputFormatterExceptionMessages = false;
                });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ILandingPageProvider, DefaultLandingPageProvider>();
            services.AddSingleton<IStacApiEndpointManager, StacApiEndpointManager>();
            return services;
        }

        public static IServiceCollection AddDefaultControllers(this IServiceCollection services)
        {
            services.AddSingleton<ICoreController, DefaultCoreController>();
            services.AddSingleton<ICollectionsController, DefaultCollectionsController>();
            services.AddSingleton<IItemSearchController, DefaultItemSearchController>();
            services.AddSingleton<IFeaturesController, DefaultFeaturesController>();
            services.AddSingleton<IFilterController, DefaultFilterController>();
            services.AddSingleton<ITransactionController, DefaultTransactionController>();
            return services;
        }

        public static IServiceCollection AddExtensionsConstraints(this IServiceCollection services)
        {
            services.AddRouting(options =>
                options.ConstraintMap.Add("FilterExtensionSearch", typeof(FilterExtensionSearchConstraint)));
            return services;
        }

        public static IServiceCollection AddCodeGenOptions(this IServiceCollection services, IConfigurationSection configurationSection)
        {
            services.Configure<CodeGenOptions>(configurationSection);
            return services;
        }

        public static void ConfigureProblemDetails(Hellang.Middleware.ProblemDetails.ProblemDetailsOptions options, IConfiguration configuration, IHostEnvironment env)
        {
            // This is the default behavior; only include exception details in a development environment.
            options.IncludeExceptionDetails = (ctx, ex) => env.IsDevelopment() || configuration.GetValue<bool>("IncludeExceptionDetails");

            // This will map NotImplementedException to the 501 Not Implemented status code.
            options.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);

            // This will map HttpRequestException to the 503 Service Unavailable status code.
            options.MapToStatusCode<HttpRequestException>(StatusCodes.Status503ServiceUnavailable);

            options.MapToStatusCode<InvalidOperationException>(StatusCodes.Status400BadRequest);

            options.MapToStatusCode<UnauthorizedAccessException>(StatusCodes.Status403Forbidden);

            options.MapToStatusCode<DirectoryNotFoundException>(StatusCodes.Status404NotFound);

            options.MapToStatusCode<FileNotFoundException>(StatusCodes.Status404NotFound);

            // Because exceptions are handled polymorphically, this will act as a "catch all" mapping, which is why it's added last.
            // If an exception other than NotImplementedException and HttpRequestException is thrown, this will handle it.
            options.MapToStatusCode<System.Exception>(StatusCodes.Status500InternalServerError);
        }


    }
}