using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

    }
}