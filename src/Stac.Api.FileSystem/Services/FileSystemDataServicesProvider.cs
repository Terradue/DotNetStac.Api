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

        public ICollectionsProvider GetCollectionsProvider()
        {
            // Generate a new instance of the collection provider
            return ActivatorUtilities.CreateInstance<FileSystemCollectionsProvider>(_serviceProvider);
        }

        public IItemsBroker GetItemsBroker()
        {
            // Generate a new instance of the items broker
            var itemsBroker = ActivatorUtilities.CreateInstance<StacFileSystemItemsBroker>(_serviceProvider);
            return itemsBroker;
        }

        public IItemsProvider GetItemsProvider()
        {
            // Generate a new instance of the items provider
            var itemsProvider = ActivatorUtilities.CreateInstance<FileSystemItemsProvider>(_serviceProvider);
            return itemsProvider;
        }

        public IRootCatalogProvider GetRootCatalogProvider()
        {
            // Generate a new instance of the root catalog provider
            return ActivatorUtilities.CreateInstance<FileSystemRootCatalogProvider>(_serviceProvider);
        }
    }
}