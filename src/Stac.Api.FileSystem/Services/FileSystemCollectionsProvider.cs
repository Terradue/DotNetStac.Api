using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Multiformats.Hash.Algorithms;
using Stac.Api.Interfaces;
using Stac.Api.Services.Queryable;
using Stac.Api.WebApi.Implementations.Default.Services;

namespace Stac.Api.FileSystem.Services
{
    public class FileSystemCollectionsProvider : FileSystemDataProvider<StacCollection>, ICollectionsProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private StacFileSystemResolver _fileSystemResolver;

        public FileSystemCollectionsProvider(IHttpContextAccessor httpContextAccessor,
                                             StacFileSystemResolver fileSystemResolver)
        {
            _httpContextAccessor = httpContextAccessor;
            _fileSystemResolver = fileSystemResolver;
        }

        public Task<StacCollection> GetCollectionByIdAsync(string collectionId, IStacApiContext stacApiContext, CancellationToken cancellationToken = default)
        {
            try
            {
                return Task.FromResult(
                        StacConvert.Deserialize<StacCollection>(
                            _fileSystemResolver.FileSystem.File.ReadAllText(
                                _fileSystemResolver.GetDirectory(StacFileSystemResolver.COLLECTIONS_DIR).FullName + $"/{collectionId}.json")));
            }
            catch (System.IO.IOException)
            {
                return Task.FromResult<StacCollection>(null);
            }
        }

        public Task<IEnumerable<StacCollection>> GetCollectionsAsync(IStacApiContext stacApiContext, CancellationToken cancellationToken = default)
        {
            IEnumerable<IFileInfo> collectionFiles = new List<IFileInfo>();
            try
            {
                collectionFiles = _fileSystemResolver.GetDirectory(StacFileSystemResolver.COLLECTIONS_DIR).EnumerateFiles("*.json");
            }
            catch { }

            // set the total number of collections in the context
            stacApiContext.SetMatchedItemsCount(collectionFiles.Count());

            // deserialize the collections as queryable
            var collections = collectionFiles.AsQueryable()
                                            .AsParallel()
                                            .WithCancellation(cancellationToken)
                                            .WithDegreeOfParallelism(Environment.ProcessorCount)
                                            .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                                            .Select(collectionFile =>
                                            {
                                                var collection = _fileSystemResolver.FileSystem.File.ReadAllText(collectionFile.FullName);
                                                return StacConvert.Deserialize<StacCollection>(collection);
                                            });

            // Create a queryable provider
            var _queryProvider = DefaultStacQueryProvider.CreateDefaultQueryProvider(stacApiContext, collections);
            var queryable = new StacQueryable<StacCollection>(_queryProvider, collections.AsQueryable<StacCollection>().Expression);

            return Task.FromResult(queryable as IEnumerable<StacCollection>);
        }
    }
}