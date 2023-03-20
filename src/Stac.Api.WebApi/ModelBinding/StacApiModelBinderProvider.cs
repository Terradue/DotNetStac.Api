using System.Reflection;
using GeoJSON.Net.Geometry;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Stac.Api.Interfaces;
using Stac.Api.Models.Core;

namespace Stac.Api.WebApi.ModelBinding
{
    /// <summary>
    /// Model binder provider for specific types used in STAC Core API
    /// </summary>
    internal class StacApiModelBinderProvider : IModelBinderProvider
    {

        public StacApiModelBinderProvider()
        {
        }

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

            // if this is IStacApiExtendableModel
            if (typeof(IStacApiExtendableModel).IsAssignableFrom(context.Metadata.ModelType) && context.BindingInfo.BinderType == null)
            {
                BindingInfo bindingInfo = new BindingInfo(context.BindingInfo);
                bindingInfo.BinderType = typeof(StacApiModelExtensionsModelBinder);
                // create the dault model binder
                var defaultBinder = context.CreateBinder(context.Metadata, bindingInfo);
                return new StacApiModelExtensionsModelBinder(defaultBinder, context.Services.GetService<IEnumerable<IStacApiModelExtension>>());
            }

            return null;
        }
    }
}