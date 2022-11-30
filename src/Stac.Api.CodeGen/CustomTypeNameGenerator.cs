using System.Collections.Generic;
using System.Linq;
using NJsonSchema;
using NSwag;

namespace Stac.Api.CodeGen
{
    internal class CustomTypeNameGenerator : ITypeNameGenerator
    {
        private OpenApiSpecification spec;

        ITypeNameGenerator generator = null;

        public CustomTypeNameGenerator(OpenApiSpecification spec)
        {
            this.spec = spec;
            generator = new DefaultTypeNameGenerator();
        }

        public string Generate(JsonSchema schema, string typeNameHint, IEnumerable<string> reservedTypeNames)
        {
            string typeName = typeNameHint;
            if (string.IsNullOrEmpty(typeName))
            {
                typeName = schema.Title;
            }
            if (string.IsNullOrEmpty(typeName))
            {
                var openApiSpec = schema.Parent as OpenApiComponents;
                if (openApiSpec != null)
                {
                    var originalSchema = openApiSpec.Schemas.FirstOrDefault(s => s.Value == schema);
                    if (originalSchema.Key != null)
                    {
                        typeName = originalSchema.Key;
                    }
                }
            }
            typeName = generator.Generate(schema, typeName, reservedTypeNames);
            if (spec.TypeNamesMapping.ContainsKey(typeName))
                typeName = spec.TypeNamesMapping[typeName];
            if ( reservedTypeNames.Contains(typeName) )
            {
                typeName = typeName + "2";
            }
            return typeName;
        }
    }
}