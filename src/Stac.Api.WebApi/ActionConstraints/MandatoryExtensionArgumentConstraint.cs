using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Stac.Api.WebApi.ActionConstraints
{
    /// <summary>
    /// This constraint is used to ensure that at least 1 parameter required by the extension is present
    /// This is used to ensure that the extension is not called without any parameter and thus
    /// create routing ambiguity with base API
    /// </summary>
    internal class MandatoryExtensionArgumentConstraint : IActionConstraint, IActionConstraintMetadata
    {
        private readonly ActionModel _action;

        public MandatoryExtensionArgumentConstraint(Microsoft.AspNetCore.Mvc.ApplicationModels.ActionModel action)
        {
            _action = action;
        }

        public int Order => 0;

        /// <summary>
        /// Accept the action if at least 1 parameter required by the extension is present
        /// </summary>
        /// <param name="context">Action cotext</param>
        /// <returns>True if extension parametr is present</returns>
        public bool Accept(ActionConstraintContext context)
        {
            bool anyParameterPresent = false;
            bool noParameter = true;

            // Check that at least 1 parameter required by the extension is present
            foreach(var parameter in _action.Parameters)
            {
                string parameterName = parameter.BindingInfo?.BinderModelName ?? parameter.ParameterName;
                if ( parameter.ParameterType != typeof(CancellationToken) )
                    noParameter = false;
                if (parameter.BindingInfo?.BindingSource == BindingSource.Query)
                    anyParameterPresent |= context.RouteContext.HttpContext.Request.Query.ContainsKey(parameterName);
                if (parameter.BindingInfo?.BindingSource == BindingSource.Body)
                    anyParameterPresent |= context.RouteContext.HttpContext.Request.Body != null;
            }
            return anyParameterPresent || noParameter;
        }
    }
}