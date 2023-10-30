using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Api.Attributes;
using Stac.Api.Clients.ItemSearch;
using Stac.Api.Models.Extensions.Sort.Context;
using Stac.Api.Interfaces;

namespace Stac.Api.Clients.Extensions.Sort
{
    [ConformanceClass("https://api.stacspec.org/v1.0.0-rc.2/item-search#sort")]
    public class SortStacApiExtension : IStacApiModelExtension
    {
        public string Identifier => "https://raw.githubusercontent.com/stac-api-extensions/sort/main/openapi.yaml";

        public Type ExtendedModelType => typeof(SearchBody);

        // Use the binding context to extract the sort parameters
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var searchBody = (bindingContext.Result.Model as SearchBody);
            // Read the sort parameter from the additional properties
            searchBody.AdditionalProperties.TryGetValue(DefaultSortBy.QuerySortKeyName, out var sortValue);
            if (sortValue == null)
            {
                return Task.CompletedTask;
            }
            // Get the original value
            try
            {
                var sortValueString = JsonConvert.SerializeObject(sortValue);
                Sortby sortby = JsonConvert.DeserializeObject<Sortby>(sortValueString);
                searchBody.AdditionalProperties.Remove(DefaultSortBy.QuerySortKeyName);
                searchBody.AdditionalProperties.Add(DefaultSortBy.QuerySortKeyName, sortby);
            }
            catch (Exception ex)
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, ex, bindingContext.ModelMetadata);
            }
            
            return Task.CompletedTask;
        }
    }
}