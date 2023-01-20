using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Stac.Api.Interfaces
{
    public interface IItemsBroker
    {
        Task<StacItem> CreateItemAsync(StacItem stacItem, IStacApiContext stacApiContext, CancellationToken cancellationToken);
        Task DeleteItemAsync(string featureId, IStacApiContext stacApiContext, CancellationToken cancellationToken);
        Task<ActionResult<StacItem>> UpdateItemAsync(StacItem newItem, string featureId, IStacApiContext stacApiContext, CancellationToken cancellationToken);
        Task RefreshStacCollectionAsync(IStacApiContext stacApiContext, CancellationToken cancellationToken);
    }
}