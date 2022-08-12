using Microsoft.AspNetCore.Mvc;
using NJsonSchema;
using Stac.Api.WebApi.Controllers.Fragments.Filter;

namespace Stac.Api.WebApi.Implementations
{
    public class DefaultFragmentsFilterController : DefaultBaseController, IFragmentsFilterController
    {
        public DefaultFragmentsFilterController(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }

        public Task<ActionResult<JsonSchema>> GetQueryablesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult<JsonSchema>> GetQueryablesForCollectionAsync(string collectionId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}