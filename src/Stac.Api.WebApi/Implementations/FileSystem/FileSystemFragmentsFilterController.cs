using System.IO.Abstractions;
using Microsoft.AspNetCore.Mvc;
using NJsonSchema;
using Stac.Api.WebApi.Controllers.Fragments.Filter;

namespace Stac.Api.WebApi.Implementations.FileSystem
{
    public class FileSystemFragmentsFilterController : FileSystemBaseController, IFragmentsFilterController
    {
        public FileSystemFragmentsFilterController(IHttpContextAccessor httpContextAccessor,
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