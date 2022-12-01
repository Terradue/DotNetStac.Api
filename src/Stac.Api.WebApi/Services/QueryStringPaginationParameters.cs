using Microsoft.Extensions.Primitives;
using Stac.Api.Clients.Collections;
using Stac.Api.Services.Pagination;

namespace Stac.Api.WebApi.Services
{
    internal class QueryStringPaginationParameters : IPaginationParameters
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

        public static QueryStringPaginationParameters GetPaginatorParameters(HttpContextAccessor httpContextAccessor)
        {
            StringValues limitsv = httpContextAccessor.HttpContext.Request.Query["limit"];
            int limit = limitsv.Count > 0 ? int.Parse(limitsv[0]) : 10;
            StringValues pagesv = httpContextAccessor.HttpContext.Request.Query["page"];
            int page = pagesv.Count > 0 ? int.Parse(pagesv[0]) : 1;
            StringValues startsv = httpContextAccessor.HttpContext.Request.Query["startIndex"];
            int startIndex = startsv.Count > 0 ? int.Parse(startsv[0]) : 0;
            return new QueryStringPaginationParameters(limit, page, startIndex);
        }

        internal static IEnumerable<ILinkValues> GetLinkValues(IPaginator<StacCollections> paginator, RouteValueDictionary routeValues)
        {
            List<ILinkValues> linkValues = new List<ILinkValues>();
            if (paginator.HasNextPage)
            {
                linkValues.Add(new LinkValues("next", routeValues, new Dictionary<string, object>()
                {
                    {"limit", paginator.CurrentLimit},
                    {"page", paginator.CurrentPage + 1},
                    {"startIndex", paginator.StartIndex}
                }, null, null));
            }
            if (paginator.CurrentPage > 1)
            {
                linkValues.Add(new LinkValues("prev", routeValues, new Dictionary<string, object>()
                {
                    {"limit", paginator.CurrentLimit},
                    {"page", paginator.CurrentPage - 1},
                    {"startIndex", paginator.StartIndex}
                }, null, null));
            }
            if (paginator.CurrentPage > 2)
            {
                linkValues.Add(new LinkValues("first", routeValues, new Dictionary<string, object>()
                {
                    {"limit", paginator.CurrentLimit},
                    {"page", 1},
                    {"startIndex", paginator.StartIndex}
                }, null, null));
            }
            if (paginator.TotalPages > 1 && paginator.CurrentPage < paginator.TotalPages)
            {
                linkValues.Add(new LinkValues("last", routeValues, new Dictionary<string, object>()
                {
                    {"limit", paginator.CurrentLimit},
                    {"page", paginator.TotalPages},
                    {"startIndex", paginator.StartIndex}
                }, null, null));
            }
            return linkValues;
        }
    }
}