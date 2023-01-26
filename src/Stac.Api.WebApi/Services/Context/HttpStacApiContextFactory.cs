using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Stac.Api.Interfaces;

namespace Stac.Api.WebApi.Services.Context
{
    public class HttpStacApiContextFactory : IStacApiContextFactory
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LinkGenerator _linkGenerator;
        private readonly IStacApiContextFiltersProvider _stacApiContextFilterProvider;

        public HttpStacApiContextFactory(IHttpContextAccessor httpContextAccessor,
                                         LinkGenerator linkGenerator,
                                         IStacApiContextFiltersProvider stacApiContextFilterProvider)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkGenerator = linkGenerator;
            _stacApiContextFilterProvider = stacApiContextFilterProvider;
        }

        public IEnumerable<T> ApplyContextPostQueryFilters<T>(IStacApiContext stacApiContext, IDataProvider<T> dataProvider, IEnumerable<T> items) where T : IStacObject
        {
            IEnumerable<T> filteredItems = items;
            foreach (IStacApiContextFilter stacApiContextFilter in _stacApiContextFilterProvider.GetFilters<T>())
            {
                filteredItems = stacApiContextFilter.ApplyContextPostQueryFilters(stacApiContext, dataProvider, filteredItems);
            }
            return filteredItems;
        }

        public T ApplyContextPostQueryFilters<T>(IStacApiContext stacApiContext, IDataProvider<T> dataProvider, T item) where T : IStacObject
        {
            T filteredItem = item;
            foreach (IStacApiContextFilter stacApiContextFilter in _stacApiContextFilterProvider.GetFilters<T>())
            {
                filteredItem = stacApiContextFilter.ApplyContextPostQueryFilters(stacApiContext, dataProvider, filteredItem);
            }
            return filteredItem;
        }

        public void ApplyContextPreQueryFilters<T>(IStacApiContext stacApiContext, IDataProvider<T> dataProvider) where T : IStacObject
        {
            foreach (IStacApiContextFilter stacApiContextFilter in _stacApiContextFilterProvider.GetFilters<T>())
            {
                stacApiContextFilter.ApplyContextPreQueryFilters(stacApiContext, dataProvider);
            }
        }

        public IStacApiContext Create()
        {
            HttpStacApiContext httpStacApiContext = HttpStacApiContext.Create(_httpContextAccessor.HttpContext);
            return httpStacApiContext;
        }
    }
}