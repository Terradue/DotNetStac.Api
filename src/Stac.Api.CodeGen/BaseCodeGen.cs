using System.Collections.Generic;
using NJsonSchema;
using NJsonSchema.Generation;
using NSwag;

namespace Stac.Api.CodeGen
{
    internal class BaseCodeGen
    {
        protected JsonReferenceResolver GetResolver(OpenApiDocument arg, IEnumerable<string> excludedDefinitions)
        {
            var schemaResolver = new OpenApiSchemaResolver(arg, new JsonSchemaGeneratorSettings());
            return new StacReferenceResolver(schemaResolver, excludedDefinitions);
        }
    }
}