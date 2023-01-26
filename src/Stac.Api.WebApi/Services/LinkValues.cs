using System.Runtime.Serialization;
using Microsoft.AspNetCore.Routing;
using Stac.Api.Interfaces;

namespace Stac.Api.WebApi.Services
{
    public class LinkValues : ILinkValues
    {
        public LinkValues(ILinkValues.LinkRelationType relationshipType,
                          RouteData routeData)
        {
            RelationshipType = relationshipType;
            RouteData = routeData;
        }

        public ILinkValues.LinkRelationType RelationshipType { get; }
        public RouteData RouteData { get; set; }
        public HttpMethod Method { get; set; } = HttpMethod.Get;
        public IDictionary<string, object> QueryValues { get; set; } = new Dictionary<string, object>();
        public IDictionary<string, object> HeaderValues { get; set; } = new Dictionary<string, object>();
        public IDictionary<string, object> BodyValues { get; set; } = new Dictionary<string, object>();
        public string Title { get; set; }
        public string MediaType { get; set; }
    }
}