using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Stac.Api.WebApi.Controllers.ItemSearch
{
    [JsonConverter(typeof(SearchRequestConverter))]
    public partial class SearchRequest
    {


    }
}