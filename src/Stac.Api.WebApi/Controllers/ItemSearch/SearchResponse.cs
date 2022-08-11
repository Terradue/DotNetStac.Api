using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Stac.Api.WebApi.Controllers.ItemSearch
{
    public partial class SearchResponse : Models.StacFeatureCollection
    {

        public Context Context { get; set; }

    }
}