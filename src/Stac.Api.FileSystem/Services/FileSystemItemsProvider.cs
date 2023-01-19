using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Multiformats.Hash.Algorithms;
using Stac.Api.Interfaces;
using Stac.Api.Services.Filtering;
using Stac.Api.Services.Pagination;
using Stac.Api.Services.Queryable;
using Stac.Api.WebApi.Implementations.Default.Services;
using Stac.Api.WebApi.Implementations.Shared.Geometry;

namespace Stac.Api.FileSystem.Services
{
    public class FileSystemItemsProvider : IItemsProvider, IPaginator<StacItem>
    {
        private readonly StacFileSystemResolver _fileSystemResolver;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly MultihashAlgorithm _hashAlgorithm = new MD5();

        public FileSystemItemsProvider(StacFileSystemResolver fileSystemResolver, IHttpContextAccessor httpContextAccessor)
        {
            _fileSystemResolver = fileSystemResolver;
            _httpContextAccessor = httpContextAccessor;
        }

        public bool HasNextPage { get => TotalPages > CurrentPage; }

        public int CurrentLimit { get; internal set; } = 10;

        public int CurrentPage { get; internal set; }

        public int StartIndex { get; internal set; }

        public int TotalPages { get => GetTotalResults() / CurrentLimit; }

        public string Collection { get; private set; }

        private int GetTotalResults()
        {
            return _fileSystemResolver.GetDirectory(Path.Combine(StacFileSystemResolver.COLLECTIONS_DIR, Collection, "items")).GetFiles("*.json").Length;
        }

        public void SetPaging(IPaginationParameters paginationParameters)
        {
            // Set thepagination parameters
            StartIndex = paginationParameters.StartIndex;
            CurrentPage = paginationParameters.Page;
            CurrentLimit = paginationParameters.Limit;
        }

        public Task<StacItem> GetItemByIdAsync(string featureId, CancellationToken cancellationToken)
        {
            try
            {
                return Task.FromResult(StacConvert.Deserialize<StacItem>(_fileSystemResolver.FileSystem.File.ReadAllText(_fileSystemResolver.GetDirectory(StacFileSystemResolver.COLLECTIONS_DIR).FullName + $"/{Collection}/items/{featureId}.json")));
            }
            catch (System.IO.IOException)
            {
                return Task.FromResult<StacItem>(null);
            }
        }

        public string GetItemEtag(string featureId)
        {
            var featureJson = _fileSystemResolver.FileSystem.File.ReadAllText(_fileSystemResolver.GetDirectory(StacFileSystemResolver.COLLECTIONS_DIR).FullName + $"/{Collection}/items/{featureId}.json");
            return _hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(featureJson)).ToString();
        }

        public Task<IEnumerable<StacItem>> GetItemsAsync(double[] bboxArray, DateTime? datetime, CancellationToken cancellationToken)
        {
            var itemFiles = _fileSystemResolver.GetDirectory(
                Path.Combine(StacFileSystemResolver.COLLECTIONS_DIR, Collection, "items"))
                .GetFiles("*.json");

            var items = itemFiles.Select(itemFile =>
                                            {
                                                if (cancellationToken.IsCancellationRequested)
                                                    throw new TaskCanceledException();
                                                var collection = _fileSystemResolver.FileSystem.File.ReadAllText(itemFile.FullName);
                                                return StacConvert.Deserialize<StacItem>(collection);
                                            });
            // Create a queryable provider
            var queryProvider = DefaultStacQueryProvider.CreateDefaultQueryProvider(_httpContextAccessor.HttpContext, items);
            var queryable = new StacQueryable<StacItem>(queryProvider, items.AsQueryable<StacItem>().Expression);
            queryable = queryable.Filter(bboxArray)
                                   .Filter(datetime);

            IQueryable<StacItem> genericQueryable = queryable.Skip(StartIndex + CurrentPage * CurrentLimit)
                                   .Take(CurrentLimit);

            return Task.FromResult(genericQueryable as IEnumerable<StacItem>);

        }

        public void SetCollectionParameter(string collectionId)
        {
            Collection = collectionId;
        }

    }
}