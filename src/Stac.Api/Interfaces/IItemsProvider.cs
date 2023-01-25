using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Stac.Api.Interfaces
{
    public interface IItemsProvider : IDataProvider<StacItem>
    {

        Task<StacItem> GetItemByIdAsync(string featureId, IStacApiContext stacApiContext, CancellationToken cancellationToken);
        string GetItemEtag(string featureId, IStacApiContext stacApiContext);
        Task<IEnumerable<StacItem>> GetItemsAsync(double[] bboxArray, DateTime? datetime, IStacApiContext stacApiContext, CancellationToken cancellationToken);
        bool AnyItemsExist(IEnumerable<StacItem> items, IStacApiContext stacApiContext);

    }
}