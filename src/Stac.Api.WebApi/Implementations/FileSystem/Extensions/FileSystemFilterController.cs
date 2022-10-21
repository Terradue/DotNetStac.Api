using System.IO.Abstractions;
using Microsoft.AspNetCore.Mvc;
using NJsonSchema;
using Stac.Api.WebApi.Controllers.Fragments.Filter;

namespace Stac.Api.WebApi.Implementations.FileSystem.Extensions
{
    public class FileSystemFilterController : FileSystemBaseController, Controllers.Extensions.Filter.IFilterController
    {
        public FileSystemFilterController(IHttpContextAccessor httpContextAccessor,
                                                   StacFileSystemResolver fileSystem) : base(httpContextAccessor, fileSystem)
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