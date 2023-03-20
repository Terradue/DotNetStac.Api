using System;
using System.Collections.Generic;
using Stac.Api.Extensions.Sort.Context;
using Stac.Api.Services.Queryable;

namespace Stac.Api.Clients.Extensions.Sort
{

    public partial class Sortby : IEnumerable<ISortByItem>, ISortParameters
    {
        IEnumerator<ISortByItem> IEnumerable<ISortByItem>.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    public partial class SortByItem : ISortByItem
    {
        SortDirection ISortByItem.Direction
        {
            get
            {
                switch (Direction)
                {
                    case SortByItemDirection.Asc:
                        return SortDirection.Ascending;
                    case SortByItemDirection.Desc:
                        return SortDirection.Descending;
                    default:
                        return SortDirection.Ascending;
                }
            }
        }

        public Func<T, object> GetKeySelector<T>(IEnumerable<T> items) where T : IStacObject
        {
            // Check that items is not StacQueryable
            // If it is, we can use the StacQueryable to get the StacQueryProvider
            // and use the StacQueryProvider to get the property selector
            // Otherwise, we use reflection to get the property selector
            StacQueryable<T> stacQueryable = items as StacQueryable<T>;
            if (stacQueryable != null)
            {
                return item => stacQueryable.StacQueryProvider.GetStacObjectProperty(item, Field);
            }
            else
            {
                return item => item.GetProperty(Field);
            }
        }
    }

}