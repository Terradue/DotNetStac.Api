using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Stac.Api.Interfaces;

namespace Stac.Api.FileSystem.Services
{
    public class FileSystemDataServicesProvider : IDataServicesProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public FileSystemDataServicesProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ICollectionsProvider GetCollectionsProvider(HttpContext httpContext)
        {
            // Generate a new instance of the collection provider
            return ActivatorUtilities.CreateInstance<FileSystemCollectionsProvider>(_serviceProvider);
        }

        public IItemsBroker GetItemsBroker(string collectionId, HttpContext httpContext)
        {
            // Generate a new instance of the items broker
            var itemsBroker = ActivatorUtilities.CreateInstance<StacFileSystemItemsBroker>(_serviceProvider);
            itemsBroker.SetCollectionParameter(collectionId);
            return itemsBroker;
        }

        public IItemsProvider GetItemsProvider(string collectionId, HttpContext httpContext)
        {
            // Generate a new instance of the items provider
            var itemsProvider = ActivatorUtilities.CreateInstance<FileSystemItemsProvider>(_serviceProvider);
            itemsProvider.SetCollectionParameter(collectionId);
            return itemsProvider;
        }

        public IRootCatalogProvider GetRootCatalogProvider(HttpContext httpContext)
        {
            // Generate a new instance of the root catalog provider
            return ActivatorUtilities.CreateInstance<FileSystemRootCatalogProvider>(_serviceProvider);
        }
    }
}