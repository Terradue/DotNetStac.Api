using Microsoft.Extensions.DependencyInjection;

namespace Stac.Api.WebApi.Extensions
{
    internal class StarsBuilder
    {
        private IServiceCollection _services;

        public StarsBuilder(IServiceCollection services)
        {
            _services = services;
        }
    }
}