using GeoJSON.Net.Geometry;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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

            return null;
        }
    }
}