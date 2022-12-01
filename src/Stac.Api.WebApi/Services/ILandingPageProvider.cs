using Stac.Api.Models;

namespace Stac.Api.WebApi.Services
{
    public interface ILandingPageProvider
    {
        Task<LandingPage> GetLandingPageAsync(CancellationToken cancellationToken);
    }
}