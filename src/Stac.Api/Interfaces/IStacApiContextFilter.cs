using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stac.Api.Interfaces
{
    public interface IStacApiContextFilter
    {
        void ApplyContextPreQueryFilters<T>(IStacApiContext stacApiContext, IDataProvider<T> dataProvider, IStacApiRequestBody request) where T : IStacObject;

        T ApplyContextPostQueryFilters<T>(IStacApiContext stacApiContext, IDataProvider<T> dataProvider, T item) where T : IStacObject;

        IEnumerable<T> ApplyContextPostQueryFilters<T>(IStacApiContext stacApiContext, IDataProvider<T> dataProvider, IEnumerable<T> items) where T : IStacObject;

        void ApplyContextResultFilters<T>(IStacApiContext stacApiContext, IDataProvider<T> dataProvider, IStacResultObject<T> result) where T : IStacObject;

        bool CanHandle<T>() where T : IStacObject;

        int Priority { get; }
    }
}