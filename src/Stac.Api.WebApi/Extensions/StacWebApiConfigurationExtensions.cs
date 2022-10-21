using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using Stac.Api.WebApi.Implementations.FileSystem;

namespace Stac.Api.WebApi.Extensions
{
    public static class StacWebApiConfigurationExtensions
    {
        public static IStacWebApiBuilder UseDefaultConfiguration(this IStacWebApiBuilder stacBuilder, IConfiguration configuration)
        {
            stacBuilder.Services.AddSingleton<IConfiguration>(configuration);
            return stacBuilder;
        }

        public static IStacWebApiBuilder UseFileSystemRoot(this IStacWebApiBuilder stacBuilder, string rootPath, bool createIfNotExists = false)
        {
            stacBuilder.Services.AddSingleton<IFileSystem>(new FileSystem());
            stacBuilder.Services.AddSingleton<StacFileSystemResolver>(sp => {
                var stacFileSystem = new StacFileSystemResolver(sp.GetRequiredService<IFileSystem>(), rootPath);
                if ( createIfNotExists )
                {
                    stacFileSystem.CreateRootDirIfNotExists();
                }
                return stacFileSystem;
            });
            return stacBuilder;
        }

        // public static IStacWebApiBuilder UseCredentialsOptions(this IStacWebApiBuilder starsBuilder, IConfigurationSection configurationSection)
        // {
        //     // Add Credentials from config
        //     var credOptions = configurationSection.Get<Dictionary<string, CredentialsOption>>();
        //     starsBuilder.Services.Configure<CredentialsOptions>(co => co.Load(credOptions));
        //     return starsBuilder;
        // }

        // public static IStacWebApiBuilder UseMultiS3Options(this IStacWebApiBuilder starsBuilder, IConfiguration configuration)
        // {
        //     // Add MultiS3 from config
        //     var s3Section = configuration.GetSection("S3");
        //     starsBuilder.Services.Configure<S3Options>(c =>
        //     {
        //         s3Section.Bind(c);
        //         c.ConfigurationSection = s3Section;
        //         c.RootConfiguration = configuration;
        //     });
        //     starsBuilder.Services.AddDefaultAWSOptions(configuration.GetAWSOptions());
        //     starsBuilder.Services.AddAWSService<IAmazonS3>();
        //     return starsBuilder;
        // }

        // public static IStacWebApiBuilder UsePluginsOptions(this IStacWebApiBuilder starsBuilder, IConfigurationSection configurationSection)
        // {
        //     var pluginsOptions = configurationSection.Get<PluginsOptions>();
        //     // Add Plugins from config
        //     starsBuilder.Services.Configure<PluginsOptions>(co => co.Load(pluginsOptions));
        //     return starsBuilder;
        // }
    }
}