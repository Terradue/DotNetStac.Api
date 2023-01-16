using System.Threading.Tasks;

namespace Stac.Api.Interfaces
{
    public interface IRootCatalogProvider
    {
        Task<StacCatalog> GetRootCatalogAsync();
    }
}