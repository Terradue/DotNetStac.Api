using System.Reflection;
using GeoJSON.Net.Geometry;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Stac.Api.Models.Core;

namespace Stac.Api.WebApi.ModelBinding
{
    /// <summary>
    /// Model binder provider for specific types used in STAC Core API
    /// </summary>
    internal class StacModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            Type underlyingType = Nullable.GetUnderlyingType(context.Metadata.ModelType);

            // Geometry model binding
            if (context.Metadata.ModelType == typeof(IGeometryObject))
            {
                return new GeometryModelBinder();
            }

            // Geometry filter model binding
            if (context.Metadata.ModelType == typeof(IntersectGeometryFilter))
            {
                return new GeometryFilterModelBinder<IntersectGeometryFilter>();
            }

            // Enum model binding
            if (context.Metadata.ModelType.GetTypeInfo().IsEnum || underlyingType?.GetTypeInfo().IsEnum == true)
            {
                return new LazyEnumModelBinder();
            }

            return null;
        }
    }
}