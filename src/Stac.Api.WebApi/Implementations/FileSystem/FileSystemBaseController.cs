using System.Reflection;
using Stac.Api.Models;

namespace Stac.Api.WebApi.Implementations.FileSystem
{
    public class FileSystemBaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly StacFileSystemResolver _stacFileSystem;
        protected readonly StacFileSystemReaderService _stacFileSystemReaderService;

        public Uri AppBaseUrl => new Uri($"{HttpContextAccessor.HttpContext.Request.Scheme}://{HttpContextAccessor.HttpContext.Request.Host}{HttpContextAccessor.HttpContext.Request.PathBase}");

        public LinkGenerator LinkGenerator { get; }

        public IHttpContextAccessor HttpContextAccessor => _httpContextAccessor;

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
            return _stacFileSystemReaderService.GetCatalog();
        }

        protected void CheckExists(string collectionId, string featureId)
        {
            try
            {
                _stacFileSystemReaderService.GetStacItemById(collectionId, featureId);
            }
            catch (IOException e)
            {
                throw new StacApiException(e.Message, StatusCodes.Status404NotFound, null, null, e);
            }
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

        protected StacApiLink GetSelfLink(IStacObject stacObject)
        {
            return new StacApiLink(
                new Uri(GetSelfUrl(stacObject)),
                "self",
                stacObject.Title,
                stacObject.MediaType.ToString()
            );
        }

        protected StacApiLink GetRootLink()
        {
            StacCatalog rootCatalog = _stacFileSystemReaderService.GetCatalog();
            return new StacApiLink(
                new Uri(GetSelfUrl(rootCatalog)),
                "root",
                rootCatalog.Title,
                rootCatalog.MediaType.ToString()
            );
        }

        protected StacApiLink GetCollectionLink(StacItem stacItem)
        {
            StacCollection collection = _stacFileSystemReaderService.GetCollectionById(stacItem.Collection);
            return new StacApiLink(
                new Uri(GetSelfUrl(collection)),
                "collection",
                collection.Title,
                collection.MediaType.ToString()
            );
        }

        protected string GetSelfUrl(IStacObject stacObject)
        {
            switch (stacObject)
            {
                case StacCatalog catalog:
                    return LinkGenerator.GetUriByAction(HttpContextAccessor.HttpContext, "GetLandingPage", "Core", new { });
                case StacCollection collection:
                    return LinkGenerator.GetUriByAction(HttpContextAccessor.HttpContext, "DescribeCollection", "Collections", new { collectionId = collection.Id });
                case StacItem item:
                    return LinkGenerator.GetUriByAction(HttpContextAccessor.HttpContext, "GetFeature", "Features", new { collectionId = item.Collection, featureId = item.Id });
                default:
                    throw new ArgumentOutOfRangeException(nameof(stacObject), stacObject, null);
            }

        }
    }
}