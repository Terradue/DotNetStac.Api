using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Stac.Api.Interfaces;

namespace Stac.Api.FileSystem.Services
{
    internal class FileSystemDataServicesProvider : IDataServicesProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public FileSystemDataServicesProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ICollectionsProvider GetCollectionsProvider(HttpContext httpContext)
        {
            // Generate a new instance of the collection provider
            return ActivatorUtilities.CreateInstance<ICollectionsProvider>(_serviceProvider);
        }

        public IItemsBroker GetItemsBroker(string collectionId, HttpContext httpContext)
        {
            // Generate a new instance of the items broker
            var itemsBroker = ActivatorUtilities.CreateInstance<IItemsBroker>(_serviceProvider);
            itemsBroker.SetCollectionParameter(collectionId);
            return itemsBroker;
        }

        public IItemsProvider GetItemsProvider(string collectionId, HttpContext httpContext)
        {
            // Generate a new instance of the items provider
            var itemsProvider = ActivatorUtilities.CreateInstance<IItemsProvider>(_serviceProvider);
            itemsProvider.SetCollectionParameter(collectionId);
            return itemsProvider;
        }
    }
}