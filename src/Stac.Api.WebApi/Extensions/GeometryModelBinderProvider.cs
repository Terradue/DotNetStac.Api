using GeoJSON.Net.Geometry;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Stac.Api.Models.Core;

namespace Stac.Api.WebApi.Extensions
{
    internal class GeometryModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(IGeometryObject))
            {
                return new GeometryModelBinder();
            }

            if (context.Metadata.ModelType == typeof(IntersectGeometryFilter))
            {
                return new GeometryFilterModelBinder<IntersectGeometryFilter>();
            }

            return null;
        }
    }
}