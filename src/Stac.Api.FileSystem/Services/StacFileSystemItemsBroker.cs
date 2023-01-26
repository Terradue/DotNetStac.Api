using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Stac.Api.Interfaces;
using Stac.Api.WebApi.Services;
using Stac.Collection;

namespace Stac.Api.FileSystem.Services
{
    public class StacFileSystemItemsBroker : IItemsBroker
    {
        private readonly StacFileSystemResolver _fileSystemResolver;
        private readonly IDataServicesProvider _dataServicesProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StacFileSystemItemsBroker(StacFileSystemResolver fileSystemResolver,
                                                IDataServicesProvider dataServicesProvider,
                                                IHttpContextAccessor httpContextAccessor)
        {
            _fileSystemResolver = fileSystemResolver;
            _dataServicesProvider = dataServicesProvider;
            _httpContextAccessor = httpContextAccessor;
        }

        internal Task<StacCollection> CreateStacCollectionAsync(StacCollection stacCollection, IStacApiContext stacApiContext, CancellationToken cancellationToken)
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

        public async Task RefreshStacCollectionAsync(IStacApiContext stacApiContext, CancellationToken cancellationToken)
        {
            StacCollection existingCollection = await GetStacCollectionAsync(stacApiContext.Collection, stacApiContext, cancellationToken);
            var itemsProvider = _dataServicesProvider.GetItemsProvider();

            StacCollection collection = StacCollection.Create(
                stacApiContext.Collection, stacApiContext.Collection.Titleize(),
                (await itemsProvider.GetItemsAsync(stacApiContext, cancellationToken))
                    .ToDictionary(i => new Uri($"items/{i.Id}.json", UriKind.Relative), i =>
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            throw new TaskCanceledException();
                        }
                        return i;
                    }),
                    "various");

            // remove item links
            collection.Links.Clear();

            if (existingCollection != null)
            {
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

        private async Task<StacCollection> GetStacCollectionAsync(string collection, IStacApiContext stacApiContext, CancellationToken cancellationToken)
        {
            var collectionsProviders = _dataServicesProvider.GetCollectionsProvider();
            return await collectionsProviders.GetCollectionByIdAsync(collection, stacApiContext, cancellationToken);
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

        private StacItem PrepareStacItem(StacItem stacItem, IStacApiContext stacApiContext)
        {
            StacItem fsStacItem = new StacItem(stacItem);
            ClearFileSystemLink(fsStacItem);
            fsStacItem.Collection = stacApiContext.Collection;
            return fsStacItem;
        }

        public async Task<StacItem> CreateItemAsync(StacItem stacItem, IStacApiContext stacApiContext, CancellationToken cancellationToken)
        {
            StacItem preparedItem = PrepareStacItem(stacItem, stacApiContext);
            var json = StacConvert.Serialize(preparedItem);
            var path = _fileSystemResolver.GetDirectory(StacFileSystemResolver.COLLECTIONS_DIR).FullName + $"/{stacApiContext.Collection}/items/{preparedItem.Id}.json";
            PreparePath(path);
            _fileSystemResolver.FileSystem.File.WriteAllText(path, json);
            return preparedItem;
        }

        public async Task DeleteItemAsync(string featureId, IStacApiContext stacApiContext, CancellationToken cancellationToken)
        {
            if (!_fileSystemResolver.FileSystem.File.Exists(_fileSystemResolver.GetDirectory(StacFileSystemResolver.COLLECTIONS_DIR).FullName + $"/{stacApiContext.Collection}/{featureId}.json"))
            {
                throw new FileNotFoundException("Item not found");
            }
            _fileSystemResolver.FileSystem.File.Delete(_fileSystemResolver.GetDirectory(StacFileSystemResolver.COLLECTIONS_DIR).FullName + $"/{stacApiContext.Collection}/{featureId}.json");
        }

        public async Task<ActionResult<StacItem>> UpdateItemAsync(StacItem newItem, string featureId, IStacApiContext stacApiContext, CancellationToken cancellationToken)
        {
            await DeleteItemAsync(newItem.Id, stacApiContext, cancellationToken);
            return await CreateItemAsync(newItem, stacApiContext, cancellationToken);
        }
    }
}