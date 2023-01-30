using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Routing;

namespace Stac.Api.Interfaces
{
    public interface ILinkValues
    {
        LinkRelationType RelationshipType { get; }

        HttpMethod Method { get; }

        RouteData RouteData { get; }

        IDictionary<string, object> QueryValues { get; }

        IDictionary<string, object> HeaderValues { get; }

        IDictionary<string, object> BodyValues { get; }
        string Title { get; }
        string MediaType { get; }
        bool? Merge { get; }
        string ActionName { get; }
        string ControllerName { get; }

        public enum LinkRelationType {
            [EnumMember(Value = "self")]
            Self,
            [EnumMember(Value = "next")]
            Next,
            [EnumMember(Value = "prev")]
            Previous
        }


    }
}