namespace Stac.Api.WebApi.Services
{
    public interface ICollectionsProvider
    {
        Task<StacCollection> GetCollectionByIdAsync(string collectionId, CancellationToken cancellationToken = default);

        Task<IEnumerable<StacCollection>> GetCollectionsAsync(CancellationToken cancellationToken = default);
    }
}