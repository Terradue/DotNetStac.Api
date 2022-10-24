using System.IO.Abstractions;
using System.Reflection;
using System.Text;
using Multiformats.Hash.Algorithms;
using Stac.Extensions.File;

namespace Stac.Api.WebApi.Implementations.FileSystem
{
    public class FileSystemBaseController
    {
        private const string COLLECTIONS_DIR = "collections";

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly StacFileSystemResolver _stacFileSystem;

        private readonly MultihashAlgorithm _hashAlgorithm = new MD5();

        public Uri AppBaseUrl => new Uri($"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}");

        public FileSystemBaseController(IHttpContextAccessor httpContextAccessor, StacFileSystemResolver stacFileSystem)
        {
            _httpContextAccessor = httpContextAccessor;
            _stacFileSystem = stacFileSystem;
        }

        private static readonly Assembly ThisAssembly = typeof(FileSystemBaseController).Assembly;

        protected async Task<StacCatalog> GetRootCatalogAsync(CancellationToken ct)
        {
            return StacConvert.Deserialize<StacCatalog>(GetFile("catalog.json").OpenRead());
        }

        private IFileInfo GetFile(string path)
        {
            return _stacFileSystem.FileSystem.FileInfo.FromFileName(_stacFileSystem.FileSystem.Path.Combine(_stacFileSystem.RootPath, path));
        }

        protected Uri MakeUriFromFileSystem(string path)
        {
            return new Uri(AppBaseUrl, path);
        }

        protected IEnumerable<StacCollection> GetCollections()
        {
            var collectionFiles = _stacFileSystem.GetDirectory(COLLECTIONS_DIR).GetFiles("*.json");
            foreach (var collectionFile in collectionFiles)
            {
                var collection = _stacFileSystem.FileSystem.File.ReadAllText(collectionFile.FullName);
                yield return StacConvert.Deserialize<StacCollection>(collection);
            }
        }

        protected StacCollection GetCollectionById(string collectionId)
        {
            return StacConvert.Deserialize<StacCollection>(_stacFileSystem.FileSystem.File.ReadAllText(_stacFileSystem.GetDirectory(COLLECTIONS_DIR).FullName + $"/{collectionId}.json"));
        }

        protected StacItem GetFeatureById(string collectionId, string featureId)
        {
            return StacConvert.Deserialize<StacItem>(_stacFileSystem.FileSystem.File.ReadAllText(_stacFileSystem.GetDirectory(COLLECTIONS_DIR).FullName + $"/{collectionId}/{featureId}.json"));
        }

        protected void CheckExists(string collectionId, string featureId)
        {
            if (!_stacFileSystem.FileSystem.File.Exists(_stacFileSystem.GetDirectory(COLLECTIONS_DIR).FullName + $"/{collectionId}/{featureId}.json"))
            {
                throw new StacApiException($"Feature {featureId} not found in collection {collectionId}", 404, null, null, null);
            }
        }

        protected void CheckEtag(string if_Match, string collectionId, string featureId)
        {
            var featureJson = _stacFileSystem.FileSystem.File.ReadAllText(_stacFileSystem.GetDirectory(COLLECTIONS_DIR).FullName + $"/{collectionId}/{featureId}.json");
            if (_hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(featureJson)).ToString() != if_Match)
            {
                throw new StacApiException($"Feature {featureId} in collection {collectionId} has changed", 412, null, null, null);
            }
        }

        protected Task DeleteItem(string collectionId, string featureId)
        {
            StacItem feature = GetFeatureById(collectionId, featureId);
            _stacFileSystem.FileSystem.File.Delete(_stacFileSystem.GetDirectory(COLLECTIONS_DIR).FullName + $"/{collectionId}/{featureId}.json");
            return Task.CompletedTask;
        }

        
    }
}