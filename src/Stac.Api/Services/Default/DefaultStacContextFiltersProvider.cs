using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stac.Api.Interfaces;

namespace Stac.Api.Services.Default
{
    public class DefaultStacContextFiltersProvider : IStacApiContextFiltersProvider
    {
        private readonly IEnumerable<IStacApiContextFilter> _filters;

        public DefaultStacContextFiltersProvider(IEnumerable<IStacApiContextFilter> filters)
        {
            _filters = filters;
        }

        public IEnumerable<IStacApiContextFilter> GetFilters<T>() where T : IStacObject
        {
            return _filters.Where(f => f.CanHandle<T>()).OrderBy(f => f.Priority);
        }
    }
}