using Stac.Api.Interfaces;
using Stac.Api.Services.Pagination;

namespace Stac.Api.WebApi.Services.Context
{
    internal class DefaultPaginationParameters : IPaginationParameters
    {
        public DefaultPaginationParameters()
        {
        }

        public DefaultPaginationParameters(IPaginationParameters paginationParameters)
        {
            Limit = paginationParameters?.Limit;
            Page = paginationParameters?.Page;
            Offset = paginationParameters?.Offset;
            Token = paginationParameters?.Token;
        }

        public int? Limit { get; set; }

        public int? Page { get; set; }

        public int? Offset { get; set; }

        public string Token { get; set; }
    }
}