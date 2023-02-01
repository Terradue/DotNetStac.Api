using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NJsonSchema;
using Stac.Api.Clients.Extensions.Filter;
using Stac.Api.Interfaces;
using Stac.Api.Models;
using Stac.Api.WebApi.Controllers.Extensions.Filter;
using Stac.Api.WebApi.Services;

namespace Stac.Api.WebApi.Implementations.Default.Filter
{
    public class DefaultFilterController : IFilterController
    {
        private readonly IStacApiEndpointManager _stacApiEndpointManager;
        private readonly IDataServicesProvider dataServicesProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStacLinker _stacLinker;

        public DefaultFilterController(IStacApiEndpointManager stacApiEndpointManager,
                                            IDataServicesProvider dataServicesProvider,
                                            IHttpContextAccessor httpContextAccessor,
                                            IStacLinker stacLinker)
        {
            _stacApiEndpointManager = stacApiEndpointManager;
            this.dataServicesProvider = dataServicesProvider;
            _httpContextAccessor = httpContextAccessor;
            _stacLinker = stacLinker;
        }

        public Task<ActionResult<StacFeatureCollection>> GetItemSearchAsync(FilterLang? filter_lang, Uri filter_crs, string filterParameter, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult<JsonSchema>> GetQueryablesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult<JsonSchema>> GetQueryablesForCollectionAsync(string collectionId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult<StacFeatureCollection>> PostItemSearchAsync(FilterSearchBody body, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}