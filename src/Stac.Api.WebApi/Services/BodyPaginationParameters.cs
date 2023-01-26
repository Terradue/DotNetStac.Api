using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Stac.Api.Clients.Collections;
using Stac.Api.Interfaces;

namespace Stac.Api.WebApi.Services
{
    public class BodyPaginationParameters : IPaginationParameters
    {

        public int? Limit { get; private set; }
        public int? Page { get; private set; }
        public int? Offset { get; private set; }
        public string Token { get; private set; }

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