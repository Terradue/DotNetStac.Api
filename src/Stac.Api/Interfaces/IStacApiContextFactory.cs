using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stac.Api.Models;

namespace Stac.Api.Interfaces
{
    public interface IStacApiContextFactory
    {
        IStacApiContext Create();

        void ApplyContextPreQueryFilters<T>(IStacApiContext stacApiContext, IDataProvider<T> dataProvider) where T : IStacObject;

        IEnumerable<T> ApplyContextPostQueryFilters<T>(IStacApiContext stacApiContext, IDataProvider<T> dataProvider, IEnumerable<T> items) where T : IStacObject;

        T ApplyContextPostQueryFilters<T>(IStacApiContext stacApiContext, IDataProvider<T> dataProvider, T item) where T : IStacObject;
        
        void ApplyContextResultFilters<T>(IStacApiContext stacApiContext, IDataProvider<T> dataProvider, IStacResultObject<T> result) where T : IStacObject;
    }
}