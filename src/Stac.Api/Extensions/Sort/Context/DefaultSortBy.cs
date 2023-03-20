using System.Collections.Generic;

namespace Stac.Api.Extensions.Sort.Context
{
    internal class DefaultSortBy : List<ISortByItem>, ISortParameters
    {
        public DefaultSortBy(List<ISortByItem> sortByItems) : base (sortByItems)
        {
        }
    }
}