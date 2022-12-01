using System.IO.Abstractions;
using GeoJSON.Net.Geometry;
using Microsoft.AspNetCore.Mvc;
using Stac.Api.Clients.ItemSearch;
using Stac.Api.Models;
using Stac.Api.WebApi.Controllers.ItemSearch;

namespace Stac.Api.WebApi.Implementations.FileSystem.ItemSearch
{
    public class FileSystemItemSearchController : FileSystemBaseController, IItemSearchController
    {
        public FileSystemItemSearchController(IHttpContextAccessor httpContextAccessor,
                                               LinkGenerator linkGenerator,
                                               StacFileSystemResolver fileSystem) : base(httpContextAccessor, linkGenerator, fileSystem)
        {
        }

        public Task<ActionResult<StacFeatureCollection>> GetItemSearchAsync(string bbox, IGeometryObject intersectsQueryString, string datetime, int limit, IEnumerable<string> ids, IEnumerable<string> collections, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult<StacFeatureCollection>> PostItemSearchAsync(SearchBody body, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}