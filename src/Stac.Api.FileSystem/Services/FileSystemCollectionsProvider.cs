using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Multiformats.Hash.Algorithms;
using Stac.Api.Interfaces;
using Stac.Api.Services.Pagination;

namespace Stac.Api.FileSystem.Services
{
    public class FileSystemCollectionsProvider : ICollectionsProvider, IPaginator<StacCollection>
    {
        private readonly StacFileSystemResolver _fileSystemResolver;


        public FileSystemCollectionsProvider(StacFileSystemResolver fileSystemResolver)
        {
            _fileSystemResolver = fileSystemResolver;
        }

        public bool HasNextPage { get => TotalPages > CurrentPage; }

        public int CurrentLimit { get; internal set; }

        public int CurrentPage { get; internal set; }

        public int StartIndex { get; internal set; }

        public int TotalPages { get => GetTotalResults() / CurrentLimit; }

        private int GetTotalResults()
        {
            return _fileSystemResolver.GetDirectory(StacFileSystemResolver.COLLECTIONS_DIR).GetFiles("*.json").Length;
        }

        internal IEnumerable<StacItem> GetStacItemsByCollectionId(string collectionId)
        {
            var itemFiles = _fileSystemResolver.GetDirectory(
                Path.Combine(StacFileSystemResolver.COLLECTIONS_DIR, collectionId, "items"))
                .GetFiles("*.json");
            foreach (var itemFile in itemFiles)
            {
                var collection = _fileSystemResolver.FileSystem.File.ReadAllText(itemFile.FullName);
                yield return StacConvert.Deserialize<StacItem>(collection);
            }
        }

        public Task<StacCollection> GetCollectionByIdAsync(string collectionId, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(StacConvert.Deserialize<StacCollection>(_fileSystemResolver.FileSystem.File.ReadAllText(_fileSystemResolver.GetDirectory(StacFileSystemResolver.COLLECTIONS_DIR).FullName + $"/{collectionId}.json")));
        }

        public void SetPaging(IPaginationParameters paginationParameters)
        {
            // Set thepagination parameters
            StartIndex = paginationParameters.StartIndex;
            CurrentPage = paginationParameters.Page;
            CurrentLimit = paginationParameters.Limit;
        }

        public Task<IEnumerable<StacCollection>> GetCollectionsAsync(CancellationToken cancellationToken = default)
        {
            IEnumerable<IFileInfo> collectionFiles = new List<IFileInfo>();
            try
            {
                collectionFiles = _fileSystemResolver.GetDirectory(StacFileSystemResolver.COLLECTIONS_DIR).GetFiles("*.json");
            }
            catch { }

            // return all the collections using the pagination parameters
            var collections = collectionFiles
                                .Skip(StartIndex + CurrentPage * CurrentLimit)
                                .Take(CurrentLimit)
                                .Select(collectionFile =>
                                        {
                                            if ( cancellationToken.IsCancellationRequested )
                                                    throw new TaskCanceledException();
                                            var collection = _fileSystemResolver.FileSystem.File.ReadAllText(collectionFile.FullName);
                                            return StacConvert.Deserialize<StacCollection>(collection);
                                        });

            return Task.FromResult(collections);
        }
    }
}