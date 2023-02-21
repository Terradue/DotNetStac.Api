using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Stac.Api.WebApi.Extensions
{
    internal class MandatoryExtensionArgumentConstraint : IActionConstraint, IActionConstraintMetadata
    {
        private readonly ActionModel _action;

        public MandatoryExtensionArgumentConstraint(Microsoft.AspNetCore.Mvc.ApplicationModels.ActionModel action)
        {
            _action = action;
        }

        public int Order => 0;

        public bool Accept(ActionConstraintContext context)
        {
            bool anyParameterPresent = false;

            // Check that at least 1 parameter required by the extension is present
            foreach(var parameter in _action.Parameters)
            {
                anyParameterPresent |= context.RouteContext.HttpContext.Request.Query.ContainsKey(parameter.Name);
            }
            return anyParameterPresent;
        }
    }
}