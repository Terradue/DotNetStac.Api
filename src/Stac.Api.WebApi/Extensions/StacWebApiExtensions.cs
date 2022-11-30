using Hellang.Middleware.ProblemDetails;
using Stac.Api.CodeGen;
using Stac.Api.Interfaces;
using Stac.Api.WebApi.Controllers.Children;
using Stac.Api.WebApi.Controllers.Collections;
using Stac.Api.WebApi.Controllers.Core;
using Stac.Api.WebApi.Controllers.Extensions.Filter;
using Stac.Api.WebApi.Controllers.Extensions.Transaction;
using Stac.Api.WebApi.Controllers.ItemSearch;
using Stac.Api.WebApi.Controllers.Features;
using Stac.Api.WebApi.Implementations;
using Stac.Api.WebApi.Implementations.FileSystem;
using Stac.Api.WebApi.Implementations.FileSystem.Children;
using Stac.Api.WebApi.Implementations.FileSystem.Collections;
using Stac.Api.WebApi.Implementations.FileSystem.Core;
using Stac.Api.WebApi.Implementations.FileSystem.Extensions;
using Stac.Api.WebApi.Implementations.FileSystem.ItemSearch;
using Stac.Api.WebApi.Implementations.FileSystem.Features;
using Stac.Api.WebApi.Services;

namespace Stac.Api.WebApi.Extensions
{
    public static class StacWebApiExtensions
    {
        public static IServiceCollection AddStacWebApi(this IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ILandingPageBuilder, LandingPageBuilder>();
            return services;
        }

        public static IServiceCollection AddFileSystemControllers(this IServiceCollection services, Action<IStacWebApiBuilder> configure)
        {
            services.AddSingleton<ICoreController, FileSystemCoreController>();
            services.AddSingleton<IChildrenController, FileSystemChildrenController>();
            services.AddSingleton<ICollectionsController, FileSystemCollectionsController>();
            services.AddSingleton<IFeaturesController, FileSystemFeaturesController>();
            services.AddSingleton<IItemSearchController, FileSystemItemSearchController>();
            services.AddSingleton<IFilterController, FileSystemFilterController>();
            services.AddSingleton<ITransactionController, FileSystemTransactionController>();
            // Let's Configure
            var builder = new StacWebApiBuilder(services);
            configure(builder);
            return services;
        }

        public static IServiceCollection AddCodeGenOptions(this IServiceCollection services, IConfigurationSection configurationSection)
        {
            services.Configure<CodeGenOptions>(configurationSection);
            return services;
        }

        public static void ConfigureProblemDetails(ProblemDetailsOptions options, IConfiguration configuration, IHostEnvironment env)
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