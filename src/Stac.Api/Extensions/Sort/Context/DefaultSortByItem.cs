using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Stac.Api.Services.Queryable;

namespace Stac.Api.Extensions.Sort.Context
{
    public class DefaultSortByItem : ISortByItem
    {
        private string _field;
        private SortDirection _direction;

        public DefaultSortByItem()
        {
        }

        public DefaultSortByItem(string field, SortDirection direction)
        {
            _field = field;
            _direction = direction;
        }

        [JsonProperty("field")]
        public string Field { get => _field; set => _field = value;}

        [JsonProperty("direction")]
        public SortDirection Direction { get => _direction; set => _direction = value;}

        public Func<T, object> GetKeySelector<T>(IEnumerable<T> items) where T : IStacObject
        {
            // Check that items is not StacQueryable
            // If it is, we can use the StacQueryable to get the StacQueryProvider
            // and use the StacQueryProvider to get the property selector
            // Otherwise, we use reflection to get the property selector
            StacQueryable<T> stacQueryable = items as StacQueryable<T>;
            if (stacQueryable != null)
            {
                return item => stacQueryable.StacQueryProvider.GetStacObjectProperty(item, _field);
            }
            else
            {
                return item => item.GetProperty(_field);
            }
        }
    }
}