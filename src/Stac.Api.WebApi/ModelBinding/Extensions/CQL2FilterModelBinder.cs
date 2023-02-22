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
        private JsonSerializerSettings _settings;

        public CQL2FilterModelBinder()
        {
            _settings = new JsonSerializerSettings();
            _settings.Converters.Add(new BooleanExpressionConverter());
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
                BooleanExpression booleanExpression = CreateFilter(filter_lang, value);
                bindingContext.Result = ModelBindingResult.Success(new CQL2Filter(booleanExpression));
            }
            catch (Exception ex)
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, ex, bindingContext.ModelMetadata);
            }

            return Task.CompletedTask;
        }

        private BooleanExpression CreateFilter(FilterLang? filter_lang, string filterParameter)
        {
            if (filter_lang == null || filter_lang == FilterLang.Cql2Text)
            {
                return CreateCqlFilterFromText(filterParameter);
            }
            else if (filter_lang == FilterLang.Cql2Json)
            {
                return CreateCqlFilterFromJson(filterParameter);
            }
            else
            {
                throw new ArgumentException("Invalid filter_lang value");
            }
        }

        private BooleanExpression CreateCqlFilterFromJson(string filter)
        {
            var cql = JsonConvert.DeserializeObject<BooleanExpression>(filter, _settings);
            return cql;
        }

        private BooleanExpression CreateCqlFilterFromText(string filterParameter)
        {
            throw new NotImplementedException();
        }
    }
}