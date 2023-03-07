using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Api.Clients.Extensions.Filter;
using Stac.Api.Converters;
using Stac.Api.Models.Cql2;

namespace Stac.Api.WebApi.ModelBinding.Extensions
{
    internal class CQL2FilterModelBinder : IModelBinder
    {
        public CQL2FilterModelBinder()
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
                // Get filter lang from query string
                FilterLang? filter_lang = StacAccessorsHelpers.LazyEnumParse(typeof(FilterLang), bindingContext.HttpContext.Request.Query["filter-lang"].ToString()) as FilterLang?;
                JsonSerializerSettings _settings = new JsonSerializerSettings();
                _settings.Converters.Add(new CQL2FilterConverter(filter_lang != null ? Enum.Parse<CQL2FilterConverter.FilterLang>(filter_lang.ToString()) : null));
                CQL2Filter cql2Filter = JsonConvert.DeserializeObject<CQL2Filter>(value, _settings);
                bindingContext.Result = ModelBindingResult.Success(cql2Filter);
            }
            catch (Exception ex)
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, ex, bindingContext.ModelMetadata);
            }

            return Task.CompletedTask;
        }

        
    }
}