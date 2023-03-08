using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stac.Api.Interfaces
{
    public interface IPaginator
    {
        IPaginationParameters GetNextPageParameters<T>(IStacResultObject<T> result, IStacApiContext stacApiContext) where T : IStacObject;
        IPaginationParameters GetPaginationParameters(IStacApiContext stacApiContext);
        IPaginationParameters GetPreviousPageParameters<T>(IStacResultObject<T> result, IStacApiContext stacApiContext) where T : IStacObject;
        void PreparePagination(IPaginationParameters paginationParameters, IStacApiContext paginationContext);

    }
}