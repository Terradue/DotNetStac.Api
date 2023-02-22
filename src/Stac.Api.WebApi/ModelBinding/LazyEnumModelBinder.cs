using GeoJSON.Net.Converters;
using GeoJSON.Net.Geometry;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Stac.Api.Clients.Extensions.Filter;
using Stac.Api.Converters;
using Stac.Api.Models.Core;

namespace Stac.Api.WebApi.ModelBinding
{
    internal class LazyEnumModelBinder : IModelBinder
    {
        public LazyEnumModelBinder()
        {
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
                Type underlyingType = Nullable.GetUnderlyingType(bindingContext.ModelType) ?? bindingContext.ModelType;
                var enumParsed = StacAccessorsHelpers.LazyEnumParse(underlyingType, value);
                bindingContext.Result = ModelBindingResult.Success(enumParsed);
            }
            catch (Exception ex)
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, ex, bindingContext.ModelMetadata);
            }

            return Task.CompletedTask;
        }
    }
}