using Microsoft.AspNetCore.Routing;
using Stac.Api.Interfaces;

namespace Stac.Api.WebApi.Services
{
    public class LinkValues : ILinkValues
    {
        public LinkValues(string relationshipType, RouteValueDictionary routeValues, Dictionary<string, object> queryValues, IDictionary<string, object> headerValues, IDictionary<string, object> bodyValues)
        {
            RelationshipType = relationshipType;
            RouteValues = routeValues;
            QueryValues = queryValues;
            HeaderValues = headerValues;
            BodyValues = bodyValues;
        }

        public string RelationshipType { get; }

        public RouteValueDictionary RouteValues { get; }
        
        public IDictionary<string, object> QueryValues { get; }

        public IDictionary<string, object> HeaderValues { get; }

        public IDictionary<string, object> BodyValues { get; }
    }
}