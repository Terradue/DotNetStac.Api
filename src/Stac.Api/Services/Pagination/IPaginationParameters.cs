using System;
using System.Collections.Generic;

namespace Stac.Api.Services.Pagination
{
    public interface IPaginationParameters
    {
        

        int? Limit { get; }
        int? Page { get; }
        int? Offset { get; }
        string Token { get; }

    }
}