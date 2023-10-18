using System.Collections.Generic;

namespace Stac.Api.Extensions.Sort.Context
{
    public class DefaultSortBy : List<ISortByItem>, ISortParameters
    {
        public const string QuerySortKeyName = "sortby";

        public const string SortingPropertiesKey = "SortingParameters";

        public DefaultSortBy(List<ISortByItem> sortByItems) : base (sortByItems)
        {
        }
    }
}