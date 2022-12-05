using Microsoft.Extensions.DependencyInjection;

namespace Stac.Api.WebApi.Extensions
{
    public class StacWebApiBuilder: IStacWebApiBuilder
    {
        private IServiceCollection _services;

        public StacWebApiBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public IServiceCollection Services => _services;
    }
}