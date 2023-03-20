using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stac.Api.Extensions.Sort.Context
{
    public interface ISortParameters : IEnumerable<ISortByItem>
    {
        const string QuerySortKeyName = "sortby";

        const string SortingPropertiesKey = "SortingParameters";

    }
}