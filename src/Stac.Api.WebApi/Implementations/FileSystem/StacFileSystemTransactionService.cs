using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Stac.Collection;

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
            PreparePath(path);
            _fileSystemResolver.FileSystem.File.WriteAllText(path, json);
            return Task.FromResult(stacCollection);
        }

        private StacCollection CreateFileSystemLinkedStacCollection(StacCollection stacCollection)
        {
            StacCollection fsStacCollection = new StacCollection(stacCollection);
            ClearFileSystemLink(fsStacCollection);
            return fsStacCollection;
        }

        private void ClearFileSystemLink(ILinksCollectionObject linksCollectionObject)
        {
            linksCollectionObject.Links.Where(l => l.RelationshipType == "self").ToList().ForEach(l => linksCollectionObject.Links.Remove(l));
            linksCollectionObject.Links.Where(l => l.RelationshipType == "root").ToList().ForEach(l => linksCollectionObject.Links.Remove(l));
            linksCollectionObject.Links.Where(l => l.RelationshipType == "parent").ToList().ForEach(l => linksCollectionObject.Links.Remove(l));
            linksCollectionObject.Links.Where(l => l.RelationshipType == "collection").ToList().ForEach(l => linksCollectionObject.Links.Remove(l));
        }

        internal Task<StacItem> CreateStacItemAsync(StacItem stacItem, string collectionId, CancellationToken cancellationToken)
        {
            StacItem preparedItem = PrepareStacItem(stacItem, collectionId);
            var json = StacConvert.Serialize(preparedItem);
            var path = _fileSystemResolver.GetDirectory(StacFileSystemResolver.COLLECTIONS_DIR).FullName + $"/{collectionId}/items/{preparedItem.Id}.json";
            PreparePath(path);
            _fileSystemResolver.FileSystem.File.WriteAllText(path, json);
            UpdateStacCollectionWithNewItem(collectionId, preparedItem);
            return Task.FromResult(preparedItem);
        }

        private void UpdateStacCollectionWithNewItem(string collectionId, StacItem preparedItem)
        {
            StacCollection existingCollection = null;
            try
            {
                existingCollection = _fileSystemReaderService.GetCollectionById(collectionId);
            }
            catch { }
            StacCollection collection = StacCollection.Create(
                collectionId, collectionId.Titleize(),
                _fileSystemReaderService.GetStacItemsByCollectionId(collectionId)
                    .ToDictionary(i => new Uri($"items/{i.Id}.json", UriKind.Relative), i => i),
                    "various");

            if (existingCollection != null){
                collection.Title = existingCollection.Title;
                collection.Description = existingCollection.Description;
                collection.Keywords.AddRange(existingCollection.Keywords);
                collection.License = existingCollection.License;
                collection.Providers.AddRange(existingCollection.Providers);
                collection.Assets.AddRange(existingCollection.Assets);
                collection.Links.AddRange(existingCollection.Links);
            }
            UpdateStacCollection(collection);
        }

        private void UpdateStacCollection(StacCollection collection)
        {
            var json = StacConvert.Serialize(collection);
            var path = _fileSystemResolver.GetDirectory(StacFileSystemResolver.COLLECTIONS_DIR).FullName + $"/{collection.Id}.json";
            PreparePath(path);
            _fileSystemResolver.FileSystem.File.WriteAllText(path, json);
        }

        private void PreparePath(string path)
        {
            var dir = _fileSystemResolver.FileSystem.Path.GetDirectoryName(path);
            if (!_fileSystemResolver.FileSystem.Directory.Exists(dir))
            {
                _fileSystemResolver.FileSystem.Directory.CreateDirectory(dir);
            }
        }

        private StacItem PrepareStacItem(StacItem stacItem, string collectionId)
        {
            StacItem fsStacItem = new StacItem(stacItem);
            ClearFileSystemLink(fsStacItem);
            fsStacItem.Collection = collectionId;
            return fsStacItem;
        }

        internal Task<StacItem> UpdateStacItemAsync(string if_Match, StacItem body, string collectionId, CancellationToken cancellationToken)
        {
            DeleteStacItem(collectionId, body.Id);
            return CreateStacItemAsync(body, collectionId, cancellationToken);
        }
    }
}