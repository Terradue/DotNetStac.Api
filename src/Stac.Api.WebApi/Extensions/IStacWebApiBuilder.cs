using Microsoft.Extensions.DependencyInjection;

namespace Stac.Api.WebApi.Extensions
{
    public interface IStacWebApiBuilder
    {
        // Summary:
        //     Gets the Microsoft.Extensions.DependencyInjection.IServiceCollection where Stars
        //     services are configured.
        IServiceCollection Services { get; }
    }
}