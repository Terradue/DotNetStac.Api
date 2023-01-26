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
        private readonly IStacApiContextFilterProvider _stacApiContextFilterProvider;

        public HttpStacApiContextFactory(IHttpContextAccessor httpContextAccessor,
                                         LinkGenerator linkGenerator,
                                         IStacApiContextFilterProvider stacApiContextFilterProvider)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkGenerator = linkGenerator;
            _stacApiContextFilterProvider = stacApiContextFilterProvider;
        }

        public void ApplyContextPostQueryFilters<T>(IStacApiContext stacApiContext, IDataProvider<T> dataProvider, IEnumerable<T> items) where T : IStacObject
        {
            foreach (IStacApiContextFilter stacApiContextFilter in _stacApiContextFilterProvider.GetPostQueryFilters<T>())
            {
                stacApiContextFilter.ApplyContextPostQueryFilters(stacApiContext, dataProvider, items);
            }
        }

        public void ApplyContextPostQueryFilters<T>(IStacApiContext stacApiContext, IDataProvider<T> dataProvider, T item) where T : IStacObject
        {
            foreach (IStacApiContextFilter stacApiContextFilter in _stacApiContextFilterProvider.GetPostQueryFilters<T>())
            {
                stacApiContextFilter.ApplyContextPostQueryFilters(stacApiContext, dataProvider, item);
            }
        }

        public void ApplyContextPreQueryFilters<T>(IStacApiContext stacApiContext, IDataProvider<T> dataProvider) where T : IStacObject
        {
            foreach (IStacApiContextFilter stacApiContextFilter in _stacApiContextFilterProvider.GetPreQueryFilters<T>())
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