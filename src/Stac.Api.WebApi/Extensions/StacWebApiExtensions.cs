using Microsoft.Extensions.DependencyInjection;
using Stac.Api.WebApi.Controllers;
using Stac.Api.WebApi.Implementations;

namespace Stac.Api.WebApi.Extensions
{
    public static class StacWebApiExtensions
    {
        public static IServiceCollection AddStacWebApi(this IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();
            services.AddSingleton<ICoreController, DefaultCoreController>();
            return services;
        }
    }
}