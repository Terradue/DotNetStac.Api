using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stac.Api.WebApi.Controllers.Children;

namespace Stac.Api.WebApi.Implementations.FileSystem.Children
{
    public class FileSystemChildrenController : FileSystemBaseController, IChildrenController
    {
        public FileSystemChildrenController(IHttpContextAccessor httpContextAccessor,
                                            LinkGenerator linkGenerator,
                                            StacFileSystemResolver stacFileSystem) : base(httpContextAccessor, linkGenerator, stacFileSystem)
        {
        }

        public Task<ActionResult<StacChildren>> GetChildrenAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}