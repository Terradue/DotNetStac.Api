using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stac.Api.Services.Pagination
{
    public interface IPaginator<T>
    {
        bool HasNextPage { get; }
        int CurrentLimit { get; }
        int CurrentPage { get; }
        int StartIndex { get; }
        int TotalPages { get; }

        void SetPaging(IPaginationParameters paginationParameters);
    }
}