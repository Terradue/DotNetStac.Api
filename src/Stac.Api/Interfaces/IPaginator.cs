using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stac.Api.Interfaces
{
    public interface IPaginator
    {
        IPaginationParameters GetNextPageParameters<T>(IEnumerable<T> items, IStacApiContext stacApiContext) where T : IStacObject;
        IPaginationParameters GetPaginationParameters(IStacApiContext stacApiContext);
        IPaginationParameters GetPreviousPageParameters<T>(IEnumerable<T> items, IStacApiContext stacApiContext) where T : IStacObject;
        void PreparePagination(IPaginationParameters paginationParameters, IStacApiContext paginationContext);

    }
}