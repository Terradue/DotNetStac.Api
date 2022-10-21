using System.IO.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Stac.Api.WebApi.Controllers.Fragments.Filter;
using Stac.Api.WebApi.Controllers.ItemSearch;

namespace Stac.Api.WebApi.Implementations.FileSystem
{
    public class FileSystemItemSearchController : FileSystemBaseController, IItemSearchController
    {
        public FileSystemItemSearchController(IHttpContextAccessor httpContextAccessor,
                                               StacFileSystemResolver fileSystem) : base(httpContextAccessor, fileSystem)
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