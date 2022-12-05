using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Stac.Api.Interfaces
{
    public interface ICollectionsProvider
    {
        Task<StacCollection> GetCollectionByIdAsync(string collectionId, CancellationToken cancellationToken = default);

        Task<IEnumerable<StacCollection>> GetCollectionsAsync(CancellationToken cancellationToken = default);
    }
}