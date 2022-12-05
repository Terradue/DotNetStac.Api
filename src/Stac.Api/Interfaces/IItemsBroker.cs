using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Stac.Api.Interfaces
{
    public interface IItemsBroker
    {
        Task<StacItem> CreateItemAsync(StacItem stacItem, CancellationToken cancellationToken);
        Task DeleteItemAsync(string featureId, CancellationToken cancellationToken);
        void SetCollectionParameter(string collectionId);
        Task<ActionResult<StacItem>> UpdateItemAsync(StacItem newItem, string featureId, CancellationToken cancellationToken);
    }
}