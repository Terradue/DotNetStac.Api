using System.IO.Abstractions;
using System.Reflection;

namespace Stac.Api.WebApi.Implementations.FileSystem
{
    public class FileSystemBaseController
    {
        private const string COLLECTIONS_DIR = "collections";

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly StacFileSystemResolver _stacFileSystem;

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
    }
}