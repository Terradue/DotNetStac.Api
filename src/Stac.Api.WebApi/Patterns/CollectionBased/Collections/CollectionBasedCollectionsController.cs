using System.IO.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Stac.Api.Clients.Collections;
using Stac.Api.Models;
using Stac.Api.Services.Pagination;
using Stac.Api.WebApi.Controllers.Collections;
using Stac.Api.WebApi.Services;

namespace Stac.Api.WebApi.Implementations.CollectionBased.Collections
{
    public class CollectionBasedCollectionsController : ICollectionsController, IStacLinkValuesProvider<StacCollections>
    {
        private readonly ICollectionsProvider _collectionsProvider;
        private readonly IStacLinker _stacLinker;
        private readonly HttpContextAccessor _httpContextAccessor;

        public CollectionBasedCollectionsController(ICollectionsProvider collectionsProvider,
                                                    IStacLinker stacLinker,
                                                    HttpContextAccessor httpContextAccessor)
        {
            _collectionsProvider = collectionsProvider;
            _stacLinker = stacLinker;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ActionResult<StacCollection>> DescribeCollectionAsync(string collectionId, CancellationToken cancellationToken = default)
        {
            StacCollection collection = await _collectionsProvider.GetCollectionByIdAsync(collectionId);
            if (collection == null)
            {
                return new NotFoundResult();
            }
            _stacLinker.Link(collection, _httpContextAccessor.HttpContext);
            return collection;
        }


        public async Task<ActionResult<StacCollections>> GetCollectionsAsync(CancellationToken cancellationToken = default)
        {
            if (_collectionsProvider is IPaginator<StacCollections> paginator)
            {
                paginator.SetPaging(QueryStringPaginationParameters.GetPaginatorParameters(_httpContextAccessor));
            }

            StacCollections collections = new StacCollections()
            {
                Collections = (await _collectionsProvider.GetCollectionsAsync()).Select(c =>
                {
                    _stacLinker.Link(c, _httpContextAccessor.HttpContext);
                    return c;
                }).ToList()
            };
            _stacLinker.Link(collections, _httpContextAccessor.HttpContext);

            // Add paging links
            if (_collectionsProvider is IPaginator<StacCollections> paginator2)
            {
                _stacLinker.Link<StacCollections>(collections, this, _httpContextAccessor.HttpContext);
            }

            return collections;
        }

        public IEnumerable<ILinkValues> GetLinkValues()
        {
            List<ILinkValues> linkValues = new List<ILinkValues>();
            if ( _collectionsProvider is IPaginator<StacCollections> paginator)
            {
                linkValues.AddRange(QueryStringPaginationParameters.GetLinkValues(paginator, null);
            }
            return linkValues;
        }
    }
}