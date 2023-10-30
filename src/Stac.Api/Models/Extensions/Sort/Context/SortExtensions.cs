using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace Stac.Api.Models.Extensions.Sort.Context
{
    public static class SortExtensions
    {

        public static List<ISortByItem> ParseSortByParameters(StringValues sorts)
        {
            // Parse the sort parameters
            List<ISortByItem> sortByItems = new List<ISortByItem>();
            foreach (string sort in sorts)
            {
                // check if first char is a minus, plus or nothing
                if (sort.StartsWith("-"))
                {
                    sortByItems.Add(new DefaultSortByItem(sort.Substring(1), SortDirection.Descending));
                }
                else if (sort.StartsWith("+"))
                {
                    sortByItems.Add(new DefaultSortByItem(sort.Substring(1), SortDirection.Ascending));
                }
                else
                {
                    sortByItems.Add(new DefaultSortByItem(sort, SortDirection.Ascending));
                }
            }
            return sortByItems;
        }

        internal static ISortParameters MergeParameters(this ISortParameters sortParameters1, ISortParameters sortParameters2)
        {
            // Merge the sort parameters
            List<ISortByItem> sortByItems = new List<ISortByItem>();
            // Add the sort parameters from the first list
            if (sortParameters1 != null && sortParameters1.Any())
            {
                sortByItems.AddRange(sortParameters1);
            }
            // Remove the sort parameters that are already in the list
            sortByItems.RemoveAll(sortByItem => sortParameters2.Any(sortByItem2 => sortByItem2.Field == sortByItem.Field));
            // Add the sort parameters from the second list
            if (sortParameters2 != null && sortParameters2.Any())
            {
                sortByItems.AddRange(sortParameters2);
            }
            return new DefaultSortBy(sortByItems);
        }
    }
}