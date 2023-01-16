using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Multiformats.Hash.Algorithms;
using Stac.Api.Interfaces;
using Stac.Api.Services.Pagination;
using Stac.Api.Services.Queryable;
using Stac.Api.WebApi.Implementations.Default.Services;
using Stac.Api.WebApi.Services;

namespace Stac.Api.FileSystem.Services
{
    public class FileSystemRootCatalogProvider : IRootCatalogProvider
    {
        private StacFileSystemResolver _fileSystemResolver;

        public FileSystemRootCatalogProvider(StacFileSystemResolver fileSystemResolver)
        {
            _fileSystemResolver = fileSystemResolver;
        }

        public Task<StacCatalog> GetRootCatalogAsync()
        {
            return Task.FromResult(StacConvert.Deserialize<StacCatalog>(_fileSystemResolver.FileSystem.File.ReadAllText(_fileSystemResolver.GetRootDirectory().FullName + "/catalog.json")));
        }
    }
}