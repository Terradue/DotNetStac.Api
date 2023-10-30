using System.Collections.Generic;
using Stac.Api.Interfaces;

namespace Stac.Api.Models.Extensions.Sort.Context
{
    public interface ISortExtension
    {
        IEnumerable<T> ApplySorting<T>(IStacApiContext stacApiContext, IDataProvider<T> dataProvider, IEnumerable<T> items) where T : IStacObject;
        
        void PrepareSorting(IStacApiContext stacApiContext, IStacApiRequestBody request);
    }
}