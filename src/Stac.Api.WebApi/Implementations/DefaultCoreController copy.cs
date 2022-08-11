using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stac.Api.Models;
using Stac.Api.WebApi.Controllers;
using Stac.Api.WebApi.Controllers.Fragments.Filter;
using Stac.Api.WebApi.Controllers.ItemSearch;

namespace Stac.Api.WebApi.Implementations
{
    public class DefaultItemSearchController : DefaultBaseController, IItemSearchController
    {
        public DefaultItemSearchController(IHttpContextAccessor httpContextAccessor): base (httpContextAccessor)
        {
        }

        public Task<ActionResult<SearchResponse>> GetItemSearchAsync(string bbox, IntersectsQueryString intersectsQueryString, string datetime, int limit, IEnumerable<string> ids, IEnumerable<string> collections, string fields, string sortby, FilterParameter filterParameter, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult<SearchResponse>> PostItemSearchAsync(SearchRequest body, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}