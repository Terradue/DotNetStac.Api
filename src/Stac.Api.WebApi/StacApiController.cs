using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Stac.Api.Attributes;
using Stac.Api.Interfaces;

namespace Stac.Api.WebApi
{
    public abstract class StacApiController : ControllerBase, IStacApiController
    {

        protected StacApiController()
        {
        }

        public virtual object GetActionParameters(string actionName){
            return null;
        }

        public virtual IReadOnlyCollection<string> GetConformanceClasses()
        {
            System.Attribute[] ccAttrs = System.Attribute.GetCustomAttributes(this.GetType()).Where(a => a is ConformanceClassAttribute).ToArray();
            List<string> conformanceClasses = new List<string>();
            foreach (ConformanceClassAttribute ccAttr in ccAttrs)
            {
                conformanceClasses.Add(ccAttr.ConformanceClass);
            }
            return conformanceClasses;
        }

        public IReadOnlyCollection<StacLink> GetLandingPageLinks(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
        {
            System.Attribute[] lpaAttrs = System.Attribute.GetCustomAttributes(this.GetType()).Where(a => a is LandingPageActionAttribute).ToArray();
            List<StacLink> landingPageActionLinks = new List<StacLink>();
            foreach (LandingPageActionAttribute lpaAttr in lpaAttrs)
            {
                landingPageActionLinks.Add(lpaAttr.GetStacLink(linkGenerator, httpContextAccessor, this.GetType().Name.Replace("Controller", ""), this.GetActionParameters(lpaAttr.Action)));
            }
            return landingPageActionLinks;
        }
    }
}