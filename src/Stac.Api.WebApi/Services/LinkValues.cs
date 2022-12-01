namespace Stac.Api.WebApi.Services
{
    internal class LinkValues : ILinkValues
    {
        public LinkValues(string v, RouteValueDictionary routeValues, Dictionary<string, object> queryValues, IDictionary<string, object> headerValues, IDictionary<string, object> bodyValues)
        {
            V = v;
            RouteValues = routeValues;
            QueryValues = queryValues;
            HeaderValues = headerValues;
            BodyValues = bodyValues;
        }

        public string RelationshipType { get; }

        public RouteValueDictionary RouteValues { get; }
        
        IDictionary<string, object> QueryValues { get; }

        IDictionary<string, object> HeaderValues { get; }

        IDictionary<string, object> BodyValues { get; }
    }
}