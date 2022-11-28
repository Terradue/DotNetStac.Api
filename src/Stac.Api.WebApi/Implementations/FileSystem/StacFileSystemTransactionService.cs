using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Stac.Api.WebApi.Implementations.FileSystem
{
    public class StacFileSystemTransactionService
    {
        private readonly StacFileSystemResolver _fileSystemResolver;
        private StacFileSystemReaderService _fileSystemReaderService;

        public StacFileSystemTransactionService(StacFileSystemResolver fileSystemResolver)
        {
            _fileSystemResolver = fileSystemResolver;
            _fileSystemReaderService = new StacFileSystemReaderService(fileSystemResolver);
        }

        public Task DeleteStacItem(string collectionId, string featureId)
        {
            StacItem feature = _fileSystemReaderService.GetStacItemById(collectionId, featureId);
            _fileSystemResolver.FileSystem.File.Delete(_fileSystemResolver.GetDirectory(StacFileSystemResolver.COLLECTIONS_DIR).FullName + $"/{collectionId}/{featureId}.json");
            return Task.CompletedTask;
        }

        internal Task<StacCollection> CreateStacCollectionAsync(StacCollection stacCollection, CancellationToken cancellationToken)
        {
            var json = StacConvert.Serialize(CreateFileSystemLinkedStacCollection(stacCollection));
            var path = _fileSystemResolver.GetDirectory(StacFileSystemResolver.COLLECTIONS_DIR).FullName + $"/{stacCollection.Id}.json";
            _fileSystemResolver.FileSystem.File.WriteAllText(path, json);
            return Task.FromResult(stacCollection);
        }

        private StacCollection CreateFileSystemLinkedStacCollection(StacCollection stacCollection)
        {
            StacCollection fsStacCollection = new StacCollection(stacCollection);
            ClearFileSystemLink(fsStacCollection.Links);
            return fsStacCollection;
        }

        private void ClearFileSystemLink(ICollection<StacLink> links)
        {
            links.Where(l => l.RelationshipType == "self").ToList().ForEach(l => links.Remove(l));
            links.Where(l => l.RelationshipType == "root").ToList().ForEach(l => links.Remove(l));
            links.Where(l => l.RelationshipType == "parent").ToList().ForEach(l => links.Remove(l));
        }

        internal Task<StacItem> CreateStacItemAsync(StacItem stacItem, string collectionId, CancellationToken cancellationToken)
        {
            var json = StacConvert.Serialize(CreateFileSystemLinkedStacItem(stacItem));
            var path = _fileSystemResolver.GetDirectory(StacFileSystemResolver.COLLECTIONS_DIR).FullName + $"/{collectionId}/{stacItem.Id}.json";
            _fileSystemResolver.FileSystem.File.WriteAllText(path, json);
            return Task.FromResult(stacItem);
        }

        private StacItem CreateFileSystemLinkedStacItem(StacItem stacItem)
        {
            StacItem fsStacItem = new StacItem(stacItem);
            ClearFileSystemLink(fsStacItem.Links);
            return fsStacItem;
        }

        internal Task<StacItem> UpdateStacItemAsync(string if_Match, StacItem body, string collectionId, CancellationToken cancellationToken)
        {
            DeleteStacItem(collectionId, body.Id);
            return CreateStacItemAsync(body, collectionId, cancellationToken);
        }
    }
}