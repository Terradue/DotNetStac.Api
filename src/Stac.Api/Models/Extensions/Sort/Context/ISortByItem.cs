using System;
using System.Collections.Generic;

namespace Stac.Api.Models.Extensions.Sort.Context
{
    public interface ISortByItem
    {
        public string Field { get; }
        
        public SortDirection Direction { get; }

        Func<T, object> GetKeySelector<T>(IEnumerable<T> items) where T : IStacObject;
    }
}