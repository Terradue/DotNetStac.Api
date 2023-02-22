using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using NJsonSchema.Generation;

namespace Stac.Api.WebApi.Filters

{
    public class JsonErrorActionFilter : IActionFilter, IOrderedFilter
    {

        private static IDictionary<Type, NJsonSchema.JsonSchema> _schemas = new Dictionary<Type, NJsonSchema.JsonSchema>();

        public void OnActionExecuted(ActionExecutedContext context) { }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.Result == null && !context.ModelState.IsValid
                && HasJsonErrors(context.ModelState, out var jsonException))
            {
                throw new ValidationException(jsonException.Message);
            }
        }

        private static bool HasJsonErrors(ModelStateDictionary modelState, out Exception jsonException)
        {
            foreach (var entry in modelState.Values)
            {
                foreach (var error in entry.Errors)
                {
                    if (error.Exception is JsonException originalException)
                    {
                        jsonException = GetDetailedException(originalException);
                        return true;
                    }
                }
            }

            jsonException = null;
            return false;
        }

        private static Exception GetDetailedException(JsonException exception)
        {
            var settings = new JsonSchemaGeneratorSettings();
            var generator = new JsonSchemaGenerator(settings);
            var schema = generator.Generate(exception.GetType());
            // schema.Validate(exception);
            return exception;
        }

        // Set to a large negative value so it runs earlier than ModelStateInvalidFilter
        public int Order => -200000;
    }
}