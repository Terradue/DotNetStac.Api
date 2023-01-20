using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Stac.Api.Clients.Collections;
using Stac.Api.Interfaces;

namespace Stac.Api.WebApi.Services
{
    public class QueryStringPaginationParameters : IPaginationParameters
    {
        public QueryStringPaginationParameters(int limit, int page, int startIndex)
        {
            Limit = limit;
            Page = page;
            StartIndex = startIndex;
        }

        public int Limit { get; }
        public int Page { get; }
        public int StartIndex { get; }

        public static QueryStringPaginationParameters GetPaginatorParameters(HttpContext httpContext)
        {
            StringValues limitsv = httpContext.Request.Query["limit"];
            int limit = limitsv.Count > 0 ? int.Parse(limitsv[0]) : 10;
            StringValues pagesv = httpContext.Request.Query["page"];
            int page = pagesv.Count > 0 ? int.Parse(pagesv[0]) : 1;
            StringValues startsv = httpContext.Request.Query["startIndex"];
            int startIndex = startsv.Count > 0 ? int.Parse(startsv[0]) : 0;
            return new QueryStringPaginationParameters(limit, page, startIndex);
        }

    }
}