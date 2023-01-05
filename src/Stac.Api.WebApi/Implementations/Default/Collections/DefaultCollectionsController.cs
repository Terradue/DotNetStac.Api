using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stac.Api.Clients.Collections;
using Stac.Api.Interfaces;
using Stac.Api.Services.Pagination;
using Stac.Api.WebApi.Controllers.Collections;
using Stac.Api.WebApi.Services;

namespace Stac.Api.WebApi.Implementations.Default.Collections
{
    public class DefaultCollectionsController : ICollectionsController, IStacLinkValuesProvider<StacCollections>
    {
        private readonly IDataServicesProvider _dataServicesProvider;
        private readonly IStacLinker _stacLinker;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DefaultCollectionsController(IDataServicesProvider dataServicesProvider,
                                            IStacLinker stacLinker,
                                            IHttpContextAccessor httpContextAccessor)
        {
            _dataServicesProvider = dataServicesProvider;
            _stacLinker = stacLinker;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ActionResult<StacCollection>> DescribeCollectionAsync(string collectionId, CancellationToken cancellationToken = default)
        {
            ICollectionsProvider collectionsProvider = _dataServicesProvider.GetCollectionsProvider(_httpContextAccessor.HttpContext);
            StacCollection collection = await collectionsProvider.GetCollectionByIdAsync(collectionId);
            if (collection == null)
            {
                return new NotFoundResult();
            }
            _stacLinker.Link(collection, _httpContextAccessor.HttpContext);
            return collection;
        }


        public async Task<ActionResult<StacCollections>> GetCollectionsAsync(CancellationToken cancellationToken = default)
        {
            // Get the collections provider for the context
            ICollectionsProvider collectionsProvider = _dataServicesProvider.GetCollectionsProvider(_httpContextAccessor.HttpContext);
            // Set the paging parameters if the provider is a paginator
            if (collectionsProvider is IPaginator<StacCollections> paginator)
            {
                paginator.SetPaging(QueryStringPaginationParameters.GetPaginatorParameters(_httpContextAccessor.HttpContext));
            }

            // Get collections from the provider
            var collectionsQueryable = await collectionsProvider.GetCollectionsAsync();

            // Create the collection container and link them properly
            StacCollections collections = new StacCollections()
            {
                Collections = collectionsQueryable.Select(c =>
                {
                    _stacLinker.Link(c, _httpContextAccessor.HttpContext);
                    return c;
                }).ToList()
            };
            // Link the collections container
            _stacLinker.Link(collections, _httpContextAccessor.HttpContext);

            // Add paging links
            if (collectionsProvider is IPaginator<StacCollections> paginator2)
            {
                _stacLinker.Link<StacCollections>(collections, this, _httpContextAccessor.HttpContext);
            }

            return collections;
        }

        public IEnumerable<ILinkValues> GetLinkValues()
        {
            ICollectionsProvider collectionsProvider = _dataServicesProvider.GetCollectionsProvider(_httpContextAccessor.HttpContext);
            List<ILinkValues> linkValues = new List<ILinkValues>();
            if ( collectionsProvider is IPaginator<StacCollections> paginator)
            {
                linkValues.AddRange(QueryStringPaginationParameters.GetLinkValues(paginator, null));
            }
            return linkValues;
        }
    }
}