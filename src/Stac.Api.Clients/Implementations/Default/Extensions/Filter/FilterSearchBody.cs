using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Stac.Api.Clients.Converters;

namespace Stac.Api.Clients.Extensions.Filter
{
    [JsonConverter(typeof(FilterSearchBodyConverter))]
    public partial class FilterSearchBody
    {
        
    }
}