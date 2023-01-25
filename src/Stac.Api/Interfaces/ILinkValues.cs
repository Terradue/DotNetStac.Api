using System.Collections.Generic;
using Microsoft.AspNetCore.Routing;

namespace Stac.Api.Interfaces
{
    public interface ILinkValues
    {
        string RelationshipType { get; }

        RouteValueDictionary RouteValues { get; }

        IDictionary<string, object> QueryValues { get; }

        IDictionary<string, object> HeaderValues { get; }

        IDictionary<string, object> BodyValues { get; }
    }
}