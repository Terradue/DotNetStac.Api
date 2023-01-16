using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Stac.Api.Interfaces
{
    public interface IItemsProvider
    {
        string Collection { get; }

        Task<StacItem> GetItemByIdAsync(string featureId, CancellationToken cancellationToken);
        string GetItemEtag(string featureId);
        Task<IEnumerable<StacItem>> GetItemsAsync(double[] bboxArray, DateTime? datetime, CancellationToken cancellationToken);
        void SetCollectionParameter(string collectionId);

    }
}