using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stac.Api.Interfaces;

namespace Stac.Api.Services.Pagination
{
    public static class PaginationExtensions
    {
        public static IPaginationParameters GetPaginationParameters(this IStacApiContext stacApiContext)
        {
            return stacApiContext.GetProperty<IPaginationParameters>(IPaginationParameters.PaginationPropertiesKey);
        }
    }
}