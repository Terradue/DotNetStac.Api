using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Stac.Api.WebApi.ActionConstraints;
using Stac.Api.WebApi.Attributes;
using Stac.Api.WebApi.Extensions;

namespace Stac.Api.WebApi.ApplicationModels
{
    /// <summary>
    /// This convention is used to add constraint to STAC API extension actions
    /// </summary>
    internal class StacApiExtensionsConvention : IActionModelConvention
    {
        public void Apply(ActionModel action)
        {
            var stacExtension = action.Controller.Attributes.OfType<StacApiControllerExtensionAttribute>().FirstOrDefault();

            // If the action is in an extension controler, 
            if (stacExtension != null)
            {
                foreach (var selector in action.Selectors)
                {
                    // we add a contraint to check the arguments of the extension
                    selector.ActionConstraints.Add(new MandatoryExtensionArgumentConstraint(action));
                }
            }
        }
    }
}