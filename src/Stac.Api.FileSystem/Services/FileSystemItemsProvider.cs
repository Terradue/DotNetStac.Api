using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Multiformats.Hash.Algorithms;
using Stac.Api.Interfaces;
using Stac.Api.Services.Filtering;
using Stac.Api.Services.Queryable;
using Stac.Api.WebApi.Implementations.Default.Services;
using Stac.Api.WebApi.Implementations.Shared.Geometry;

namespace Stac.Api.FileSystem.Services
{
    public class FileSystemItemsProvider : FileSystemDataProvider<StacItem>, IItemsProvider
    {
        private readonly StacFileSystemResolver _fileSystemResolver;
        private readonly MultihashAlgorithm _hashAlgorithm = new MD5();

        public FileSystemItemsProvider(StacFileSystemResolver fileSystemResolver)
        {
            _fileSystemResolver = fileSystemResolver;
        }

        public Task<StacItem> GetItemByIdAsync(string featureId, IStacApiContext stacApiContext, CancellationToken cancellationToken)
        {
            try
            {
                return Task.FromResult(StacConvert.Deserialize<StacItem>(
                        _fileSystemResolver.FileSystem.File.ReadAllText(
                                _fileSystemResolver.GetDirectory(
                                    StacFileSystemResolver.COLLECTIONS_DIR).FullName + $"/{stacApiContext.Collection}/items/{featureId}.json")));
            }
            catch (System.IO.IOException)
            {
                return Task.FromResult<StacItem>(null);
            }
        }

        public string GetItemEtag(string featureId, IStacApiContext stacApiContext)
        {
            var featureJson = _fileSystemResolver.FileSystem.File.ReadAllText(
                        _fileSystemResolver.GetDirectory(
                            StacFileSystemResolver.COLLECTIONS_DIR).FullName + $"/{stacApiContext.Collection}/items/{featureId}.json");
            return _hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(featureJson)).ToString();
        }

        public Task<IEnumerable<StacItem>> GetItemsAsync(double[] bboxArray, DateTime? datetime, IStacApiContext stacApiContext, CancellationToken cancellationToken)
        {
            IEnumerable<IFileInfo> itemFiles = _fileSystemResolver.GetDirectory(
                Path.Combine(StacFileSystemResolver.COLLECTIONS_DIR, stacApiContext.Collection, "items"))
                .GetFiles("*.json").ToList();

            // Set the total number of items in the context
            stacApiContext.SetMatchedItemsCount(itemFiles.Count());

            // if the filters are null, we can paginate the items directly to speed up the process
            // if (bboxArray == null && datetime == null && stacApiContext.PaginationParameters != null)
            // {
            //     itemFiles = itemFiles.AsQueryable().Skip(stacApiContext.PaginationParameters.StartIndex + ((stacApiContext.PaginationParameters.Page -1) * stacApiContext.PaginationParameters.Limit))
            //                          .Take(stacApiContext.PaginationParameters.Limit);
            // }

            var items = itemFiles.Select(itemFile =>
                                            {
                                                if (cancellationToken.IsCancellationRequested)
                                                    throw new TaskCanceledException();
                                                var collection = _fileSystemResolver.FileSystem.File.ReadAllText(itemFile.FullName);
                                                return StacConvert.Deserialize<StacItem>(collection);
                                            });

            // Create a queryable provider
            var queryProvider = DefaultStacQueryProvider.CreateDefaultQueryProvider(stacApiContext, items);
            var queryable = new StacQueryable<StacItem>(queryProvider, items.AsQueryable<StacItem>().Expression);

            IQueryable<StacItem> genericQueryable = queryable;

            // Apply the filters if they are not null
            if (bboxArray != null || datetime != null)
            {
                queryable = queryable.Filter(bboxArray)
                                     .Filter(datetime);

                // if (stacApiContext.PaginationParameters != null)
                // {
                //     genericQueryable = queryable.Skip(stacApiContext.PaginationParameters.StartIndex + ((stacApiContext.PaginationParameters.Page -1) * stacApiContext.PaginationParameters.Limit))
                //                                 .Take(stacApiContext.PaginationParameters.Limit);
                // }
            }

            return Task.FromResult(genericQueryable as IEnumerable<StacItem>);

        }

        public bool AnyItemsExist(IEnumerable<StacItem> items, IStacApiContext stacApiContext)
        {
            try
            {
                var files = _fileSystemResolver.GetDirectory(Path.Combine(StacFileSystemResolver.COLLECTIONS_DIR, $"{stacApiContext.Collection}/items")).GetFiles("*.json");
                return items.Any(i => files.Any(f => Path.GetFileNameWithoutExtension(f.Name) == i.Id));
            }
            catch (System.IO.IOException)
            {
                return false;
            }
        }
    }
}