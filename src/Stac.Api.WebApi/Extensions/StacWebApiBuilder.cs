namespace Stac.Api.WebApi.Extensions
{
    internal class StacWebApiBuilder: IStacWebApiBuilder
    {
        private IServiceCollection _services;

        public StacWebApiBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public IServiceCollection Services => _services;
    }
}