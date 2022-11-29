using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Stac.Api.Attributes;
using Stac.Api.Interfaces;

namespace Stac.Api.WebApi
{
    public abstract class StacApiController : ControllerBase, IStacApiController
    {

        protected StacApiController()
        {
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
    }
}