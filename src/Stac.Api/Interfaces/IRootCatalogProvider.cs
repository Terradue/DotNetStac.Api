using System.Threading;
using System.Threading.Tasks;

namespace Stac.Api.Interfaces
{
    public interface IRootCatalogProvider
    {
        Task<StacCatalog> GetRootCatalogAsync(IStacApiContext stacApiContext, CancellationToken cancellationToken);
    }
}