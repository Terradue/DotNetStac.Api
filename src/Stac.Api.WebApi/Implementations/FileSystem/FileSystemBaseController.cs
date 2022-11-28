using System.IO.Abstractions;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc.Routing;
using Multiformats.Hash.Algorithms;
using Stac.Extensions.File;

namespace Stac.Api.WebApi.Implementations.FileSystem
{
    public class FileSystemBaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly StacFileSystemResolver _stacFileSystem;
        protected readonly StacFileSystemReaderService _stacFileSystemReaderService;
        

        public Uri AppBaseUrl => new Uri($"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}");

        public LinkGenerator LinkGenerator { get; }

        public FileSystemBaseController(IHttpContextAccessor httpContextAccessor,
                                        LinkGenerator linkGenerator,
                                        StacFileSystemResolver stacFileSystem)
        {
            _httpContextAccessor = httpContextAccessor;
            LinkGenerator = linkGenerator;
            _stacFileSystem = stacFileSystem;
            _stacFileSystemReaderService = new StacFileSystemReaderService(_stacFileSystem);
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

        protected void CheckExists(string collectionId, string featureId)
        {
            _stacFileSystemReaderService.GetStacItemById(collectionId, featureId);
        }

        protected void CheckEtag(string if_Match, string collectionId, string featureId)
        {
            var checksum = _stacFileSystemReaderService.GetStacItemEtagById(collectionId, featureId);
            
            if (checksum != if_Match)
            {
                throw new StacApiException($"Feature {featureId} in collection {collectionId} has changed", 412, null, null, null);
            }
        }

        protected void CheckFeatureId(StacItem body, string featureId)
        {
            throw new NotImplementedException();
        }

        protected void Relink(IStacObject c)
        {
            AddSelfLink(c);
        }

        private void AddSelfLink(IStacObject c)
        {
            LinkGenerator.GetUriByAction(_httpContextAccessor.HttpContext, "DescribeCollectionAsync", "Collections", new {collectionId = c.Id});
        }
    }
}