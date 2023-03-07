using System.Reflection;
using GeoJSON.Net.Geometry;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Stac.Api.Clients.Extensions.Filter;
using Stac.Api.Models.Core;
using Stac.Api.Models.Cql2;

namespace Stac.Api.WebApi.ModelBinding.Extensions
{
    /// <summary>
    /// Model binder provider for specific types used in STAC Core API
    /// </summary>
    internal class FilterExtensionModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            // CQL2 BooleanExpression model binding
            if (context.Metadata.ModelType == typeof(CQL2Filter))
            {
                return new CQL2FilterModelBinder();
            }

            // FilterSearchBody model binding
            if (context.Metadata.ModelType == typeof(FilterSearchBody))
            {
                return new FilterSearchBodyModelBinder();
            }

            return null;
        }
    }
}