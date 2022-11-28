using System.IO.Abstractions;
using Microsoft.AspNetCore.Mvc;
using NJsonSchema;

namespace Stac.Api.WebApi.Implementations.FileSystem.Extensions
{
    public class FileSystemFilterController : FileSystemBaseController, Controllers.Extensions.Filter.IFilterController
    {
        public FileSystemFilterController(IHttpContextAccessor httpContextAccessor,
                                          LinkGenerator linkGenerator,
                                          StacFileSystemResolver fileSystem) : base(httpContextAccessor, linkGenerator, fileSystem)
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