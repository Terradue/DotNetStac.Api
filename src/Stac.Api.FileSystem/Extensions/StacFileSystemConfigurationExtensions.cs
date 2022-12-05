using System.IO.Abstractions;
using Stac.Api.FileSystem.Services;
using Stac.Api.WebApi.Extensions;

namespace Stac.Api.FileSystem.Extensions
{
    public static class StacFileSystemConfigurationExtensions
    {

        public static IStacWebApiBuilder UseFileSystemProvider(this IStacWebApiBuilder stacBuilder, string rootPath, bool createIfNotExists = false)
        {
            stacBuilder.Services.AddSingleton<IFileSystem>(new System.IO.Abstractions.FileSystem());
            stacBuilder.Services.AddSingleton<StacFileSystemResolver>(sp => {
                var stacFileSystem = new StacFileSystemResolver(sp.GetRequiredService<IFileSystem>(), rootPath);
                if ( createIfNotExists )
                {
                    stacFileSystem.CreateRootCatalogIfNotExists();
                }
                return stacFileSystem;
            });
            return stacBuilder;
        }

       
    }
}