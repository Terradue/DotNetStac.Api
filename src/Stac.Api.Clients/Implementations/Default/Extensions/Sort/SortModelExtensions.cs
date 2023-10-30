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
    public static class SortModelExtensions
    {
        public static SearchBody SortBy(this SearchBody searchBody, Sortby sortby)
        {
            searchBody.AdditionalProperties.Add(DefaultSortBy.QuerySortKeyName, sortby);
            return searchBody;
        }
    }
}