using Stac.Api.CodeGen;
using Stac.Api.WebApi.Controllers.Collections;
using Stac.Api.WebApi.Controllers.Core;
using Stac.Api.WebApi.Controllers.OgcApiFeatures;
using Stac.Api.WebApi.Implementations;
using Stac.Api.WebApi.Implementations.FileSystem;

namespace Stac.Api.WebApi.Extensions
{
    public static class StacWebApiExtensions
    {
        public static IServiceCollection AddStacWebApi(this IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            return services;
        }

        public static IServiceCollection AddFileSystemControllers(this IServiceCollection services, Action<IStacWebApiBuilder> configure)
        {
            services.AddSingleton<ICoreController, FileSystemCoreController>();
            services.AddSingleton<ICollectionsController, FileSystemCollectionsController>();
            services.AddSingleton<IOgcApiFeaturesController, FileSystemOgcApiFeaturesController>();
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

        
    }
}