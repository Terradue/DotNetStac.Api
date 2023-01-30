using Stac.Api.Interfaces;

namespace Stac.Api.WebApi.Services.Context
{
    internal class DefaultQueryParameters : Dictionary<string, object>, IQueryParameters
    {
        public DefaultQueryParameters()
        {
        }
    }
}