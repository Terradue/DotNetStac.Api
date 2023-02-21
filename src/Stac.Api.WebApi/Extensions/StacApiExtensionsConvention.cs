using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Stac.Api.WebApi.Extensions
{
    internal class StacApiExtensionsConvention : IActionModelConvention
    {
        public void Apply(ActionModel action)
        {
            var stacExtension = action.Controller.Attributes.OfType<StacExtensionAttribute>().FirstOrDefault();

            // If the action is in an extension controler, we Add a contraint to check the arguments
            if (stacExtension != null)
            {
                foreach (var selector in action.Selectors)
                {
                    selector.ActionConstraints.Add(new MandatoryExtensionArgumentConstraint(action));
                }
            }
        }
    }
}