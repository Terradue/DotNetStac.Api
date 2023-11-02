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
                Api.Models.Cql2.FilterLang? filter_lang = StacAccessorsHelpers.LazyEnumParse(typeof(Api.Models.Cql2.FilterLang), bindingContext.HttpContext.Request.Query["filter-lang"].ToString()) as Api.Models.Cql2.FilterLang?;
                var cql2FilterConverter = new CQL2FilterConverter();
                BooleanExpression be = cql2FilterConverter.CreateFilter(JObject.Parse(value), filter_lang);
                bindingContext.Result = ModelBindingResult.Success(be);
            }
            catch (Exception ex)
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, ex, bindingContext.ModelMetadata);
            }

            return Task.CompletedTask;
        }

        
    }
}