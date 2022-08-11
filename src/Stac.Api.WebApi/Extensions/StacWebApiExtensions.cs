using Stac.Api.CodeGen;
using Stac.Api.WebApi.Controllers.Collections;
using Stac.Api.WebApi.Controllers.Core;
using Stac.Api.WebApi.Controllers.OgcApiFeatures;
using Stac.Api.WebApi.Implementations;

namespace Stac.Api.WebApi.Extensions
{
    public static class StacWebApiExtensions
    {
        public static IServiceCollection AddStacWebApi(this IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ICoreController, DefaultCoreController>();
            services.AddSingleton<ICollectionsController, DefaultCollectionsController>();
            services.AddSingleton<IOgcApiFeaturesController, DefaultOgcApiFeaturesController>();
            return services;
        }

        public static IServiceCollection AddCodeGenOptions(this IServiceCollection services, IConfigurationSection configurationSection)
        {
            services.Configure<CodeGenOptions>(configurationSection);
            return services;
        }

        
    }
}