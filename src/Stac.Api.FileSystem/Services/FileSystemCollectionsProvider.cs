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

namespace Stac.Api.FileSystem.Services
{
    public class FileSystemCollectionsProvider : ICollectionsProvider, IPaginator<StacCollection>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private StacFileSystemResolver _fileSystemResolver;

        public FileSystemCollectionsProvider(IHttpContextAccessor httpContextAccessor,
                                             StacFileSystemResolver fileSystemResolver)
        {
            _httpContextAccessor = httpContextAccessor;
            _fileSystemResolver = fileSystemResolver;
        }

        public bool HasNextPage { get => TotalPages > CurrentPage; }

        public int CurrentLimit { get; internal set; } = 10;

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
            try
            {
                return Task.FromResult(StacConvert.Deserialize<StacCollection>(_fileSystemResolver.FileSystem.File.ReadAllText(_fileSystemResolver.GetDirectory(StacFileSystemResolver.COLLECTIONS_DIR).FullName + $"/{collectionId}.json")));
            }
            catch (System.IO.IOException)
            {
                return Task.FromResult<StacCollection>(null);
            }
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
                collectionFiles = _fileSystemResolver.GetDirectory(StacFileSystemResolver.COLLECTIONS_DIR).EnumerateFiles("*.json");
            }
            catch { }

            // return all the collections using the pagination parameters
            var collections = collectionFiles
                                .Select(collectionFile =>
                                        {
                                            if (cancellationToken.IsCancellationRequested)
                                                throw new TaskCanceledException();
                                            var collection = _fileSystemResolver.FileSystem.File.ReadAllText(collectionFile.FullName);
                                            return StacConvert.Deserialize<StacCollection>(collection);
                                        });

            // Create a queryable provider
            var _queryProvider = DefaultStacQueryProvider.CreateDefaultQueryProvider(_httpContextAccessor.HttpContext, collections);
            var queryable = new StacQueryable<StacCollection>(_queryProvider, collections.AsQueryable<StacCollection>().Expression);
            var genericQueryable = queryable.Skip(StartIndex + CurrentPage * CurrentLimit).Take(CurrentLimit);

            return Task.FromResult(genericQueryable as IEnumerable<StacCollection>);
        }
    }
}