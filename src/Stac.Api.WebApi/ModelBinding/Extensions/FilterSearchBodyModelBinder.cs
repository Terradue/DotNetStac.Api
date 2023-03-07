using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Api.Clients.Converters;
using Stac.Api.Clients.Extensions.Filter;
using Stac.Api.Converters;
using Stac.Api.Models.Cql2;

namespace Stac.Api.WebApi.ModelBinding.Extensions
{
    internal class FilterSearchBodyModelBinder : IModelBinder
    {
        private JsonSerializerSettings _settings;

        public FilterSearchBodyModelBinder()
        {
            _settings = new JsonSerializerSettings();
            _settings.Converters.Add(new FilterSearchBodyConverter());
        }

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            // read body stream
            var request = bindingContext.HttpContext.Request;
            string value;
            using (var reader = new StreamReader(request.Body))
            {
                value = await reader.ReadToEndAsync();
            }

            try
            {
                FilterSearchBody filterSearchBody = JsonConvert.DeserializeObject<FilterSearchBody>(value, _settings);
                bindingContext.Result = ModelBindingResult.Success(filterSearchBody);
            }
            catch (Exception ex)
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, ex, bindingContext.ModelMetadata);
            }
        }
    }
}