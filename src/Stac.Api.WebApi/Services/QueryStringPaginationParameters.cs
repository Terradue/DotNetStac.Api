using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Stac.Api.Clients.Collections;
using Stac.Api.Interfaces;
using Stac.Api.Services.Pagination;

namespace Stac.Api.WebApi.Services
{
    public class QueryStringPaginationParameters : IPaginationParameters
    {
        

        public QueryStringPaginationParameters()
        {
        }

        public QueryStringPaginationParameters(IPaginationParameters paginationParameters)
        {
            Limit = paginationParameters.Limit;
            Page = paginationParameters.Page;
            Offset = paginationParameters.Offset;
            Token = paginationParameters.Token;
        }

        public int? Limit { get; set; }
        public int? Page { get; set; }
        public int? Offset { get; set; }
        public string Token { get; set; }

        public static QueryStringPaginationParameters GetPaginatorParameters(HttpContext httpContext)
        {
            QueryStringPaginationParameters paginatorParameters = new QueryStringPaginationParameters();
            StringValues limitsv = httpContext.Request.Query["limit"];
            if (limitsv.Count > 0)
            {
                paginatorParameters.Limit = int.Parse(limitsv[0]);
            }
            StringValues pagesv = httpContext.Request.Query["page"];
            if (pagesv.Count > 0)
            {
                paginatorParameters.Page = int.Parse(pagesv[0]);
            }
            StringValues startsv = httpContext.Request.Query["offset"];
            if (startsv.Count > 0)
            {
                paginatorParameters.Offset = int.Parse(startsv[0]);
            }
            StringValues tokensv = httpContext.Request.Query["token"];
            if (tokensv.Count > 0)
            {
                paginatorParameters.Token = tokensv[0];
            }
            
            return paginatorParameters;
        }

    }
}