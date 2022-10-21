using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace Stac.Api.WebApi.Implementations.FileSystem
{
    public class StacFileSystemResolver
    {
        private readonly IFileSystem _fileSystem;

        public string RootPath { get; }
        

        public StacFileSystemResolver(IFileSystem fileSystem, string rootPath)
        {
            _fileSystem = fileSystem;
            RootPath = rootPath;
        }

        public IFileSystem FileSystem => _fileSystem;

        public IDirectoryInfo GetRootDirectory()
        {
            return _fileSystem.DirectoryInfo.FromDirectoryName(RootPath);
        }

        internal void CreateRootCatalogIfNotExists()
        {
            if ( ! GetRootDirectory().Exists)
            {
                GetRootDirectory().Create();
            }
            StacCatalog rootCatalog = new StacCatalog("root", "Root catalog");
            
            _fileSystem.File.WriteAllText(GetRootDirectory().FullName + "/catalog.json", StacConvert.Serialize(rootCatalog));
        }

        internal IDirectoryInfo GetDirectory(string path)
        {
            return _fileSystem.DirectoryInfo.FromDirectoryName(_fileSystem.Path.Combine(RootPath, path));
        }
    }
}