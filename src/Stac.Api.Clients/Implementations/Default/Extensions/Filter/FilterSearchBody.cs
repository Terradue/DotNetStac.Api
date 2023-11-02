using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Stac.Api.Clients.Converters;
using Stac.Api.Clients.ItemSearch;
using Stac.Api.Interfaces;

namespace Stac.Api.Clients.Extensions.Filter
{
    [JsonConverter(typeof(FilterSearchBodyConverter))]
    public partial class FilterSearchBody : ISearchExpression
    {
        public FilterSearchBody() { }

        public FilterSearchBody(SearchBody? searchBody)
        {
            if (searchBody != null)
            {
                this.Collections = searchBody.Collections;
                this.Bbox = searchBody.Bbox;
                this.Datetime = searchBody.Datetime;
                this.Ids = searchBody.Ids;
                this.Intersects = searchBody.Intersects;
                this.Limit = searchBody.Limit;
            }
        }
    }
}