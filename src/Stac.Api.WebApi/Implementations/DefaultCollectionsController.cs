using Microsoft.AspNetCore.Mvc;
using Stac.Api.WebApi.Controllers.Collections;

namespace Stac.Api.WebApi.Implementations
{
    public class DefaultCollectionsController : DefaultBaseController, ICollectionsController
    {
        public DefaultCollectionsController(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }

        public async Task<ActionResult<StacCollection>> DescribeCollectionAsync(string collectionId, CancellationToken cancellationToken = default)
        {
            var collection = GetCollections("examples/collections").FirstOrDefault(c => c.Id == collectionId);
            return collection == null ? new NotFoundResult() : (ActionResult<StacCollection>)collection;
        }

        public async Task<ActionResult<StacCollections>> GetCollectionsAsync(CancellationToken cancellationToken = default)
        {
            return new StacCollections()
            {
                Collections = GetCollections("examples/collections").ToList()
            };
        }

        
    }
}