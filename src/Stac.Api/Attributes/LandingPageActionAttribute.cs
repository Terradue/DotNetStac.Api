using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Stac.Api.Interfaces;

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

        public StacLink GetStacLink(IStacApiContext stacApiContext, string controllerName, object values)
        {
            StacLink link = new StacLink(new Uri(stacApiContext.LinkGenerator.GetUriByAction(stacApiContext.HttpContext, Action, controllerName, values)), Relationship, null, MediaType);
            if (!string.IsNullOrEmpty(Method))
            {
                link.AdditionalProperties = new Dictionary<string, object>();
                link.AdditionalProperties.Add("method", Method);
            }
            return link;
        }
    }

}