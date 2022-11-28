using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Multiformats.Hash.Algorithms;

namespace Stac.Api.WebApi.Implementations.FileSystem
{
    public class StacFileSystemReaderService
    {
        private readonly StacFileSystemResolver _fileSystemResolver;

        private readonly MultihashAlgorithm _hashAlgorithm = new MD5();


        public StacFileSystemReaderService(StacFileSystemResolver fileSystemResolver)
        {
            _fileSystemResolver = fileSystemResolver;
        }

        public IEnumerable<StacCollection> GetCollections()
        {
            var collectionFiles = _fileSystemResolver.GetDirectory(StacFileSystemResolver.COLLECTIONS_DIR).GetFiles("*.json");
            foreach (var collectionFile in collectionFiles)
            {
                var collection = _fileSystemResolver.FileSystem.File.ReadAllText(collectionFile.FullName);
                yield return StacConvert.Deserialize<StacCollection>(collection);
            }
        }

        public StacCollection GetCollectionById(string collectionId)
        {
            return StacConvert.Deserialize<StacCollection>(_fileSystemResolver.FileSystem.File.ReadAllText(_fileSystemResolver.GetDirectory(StacFileSystemResolver.COLLECTIONS_DIR).FullName + $"/{collectionId}.json"));
        }

        public StacItem GetStacItemById(string collectionId, string featureId)
        {
            return StacConvert.Deserialize<StacItem>(_fileSystemResolver.FileSystem.File.ReadAllText(_fileSystemResolver.GetDirectory(StacFileSystemResolver.COLLECTIONS_DIR).FullName + $"/{collectionId}/{featureId}.json"));
        }

        internal string GetStacItemEtagById(string collectionId, string featureId)
        {
            var featureJson = _fileSystemResolver.FileSystem.File.ReadAllText(_fileSystemResolver.GetDirectory(StacFileSystemResolver.COLLECTIONS_DIR).FullName + $"/{collectionId}/{featureId}.json");
            return _hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(featureJson)).ToString();
        }
    }
}