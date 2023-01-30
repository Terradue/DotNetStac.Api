using GeoJSON.Net.Converters;
using GeoJSON.Net.Geometry;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Stac.Api.Converters;
using Stac.Api.Interfaces;
using Stac.Api.Models.Core;

namespace Stac.Api.WebApi.Extensions
{
    internal class GeometryFilterModelBinder<T> : IModelBinder where T : IGeometryFilter
    {
        private static JsonSerializer converter;

        public GeometryFilterModelBinder()
        {
            converter = new JsonSerializer();
            converter.Converters.Add(new GeometryFilterConverter<T>());
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

            var value = valueProviderResult.FirstValue;

            try
            {
                IGeometryFilter geometry = converter.Deserialize<IGeometryFilter>(new JsonTextReader(new System.IO.StringReader(value)));
                bindingContext.Result = ModelBindingResult.Success(geometry);
            }
            catch (Exception ex)
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, ex, bindingContext.ModelMetadata);
            }

            return Task.CompletedTask;
        }
    }
}