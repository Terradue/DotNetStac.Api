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

        internal void CreateRootDirIfNotExists()
        {
            if ( ! GetRootDirectory().Exists)
            {
                GetRootDirectory().Create();
            }
        }
    }
}