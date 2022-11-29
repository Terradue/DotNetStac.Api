using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Stac.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class LandingPageActionAttribute : Attribute
    {
        public LandingPageActionAttribute(string action, string rel, string mediaType)
        {
            Action = action;
            Relationship = rel;
            MediaType = mediaType;
        }

        public string Action { get; }
        public string Relationship { get; }
        public string MediaType { get; }
        public string Method { get; set; }


        public StacLink GetStacLink(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor, string controllerName)
        {
            StacLink link = new StacLink(new Uri(linkGenerator.GetUriByAction(httpContextAccessor.HttpContext, Action, controllerName)), Relationship, null, MediaType);
            if (!string.IsNullOrEmpty(Method))
            {
                link.AdditionalProperties = new Dictionary<string, object>();
                link.AdditionalProperties.Add("method", Method);
            }
            return link;
        }
    }

}