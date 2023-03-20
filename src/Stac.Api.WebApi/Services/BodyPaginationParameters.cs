using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Stac.Api.Clients.Collections;
using Stac.Api.Interfaces;
using Stac.Api.Services.Pagination;

namespace Stac.Api.WebApi.Services
{
    public class BodyPaginationParameters : IPaginationParameters
    {
        public BodyPaginationParameters()
        {
        }

        public BodyPaginationParameters(IPaginationParameters paginationParameters)
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

        public static BodyPaginationParameters GetPaginatorParameters(IDictionary<string, object> body)
        {
            BodyPaginationParameters paginatorParameters = new BodyPaginationParameters();
            paginatorParameters.Limit = body.GetProperty<int?>("limit");
            paginatorParameters.Page = body.GetProperty<int?>("page");
            paginatorParameters.Offset = body.GetProperty<int?>("offset");
            paginatorParameters.Token = body.GetProperty<string>("token");
            return paginatorParameters;
        }

    }
}