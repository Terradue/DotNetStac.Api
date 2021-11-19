using System.Collections.Generic;
using NJsonSchema;

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
            if ( string.IsNullOrEmpty(typeNameHint) || !spec.TypeNamesMapping.ContainsKey(typeNameHint) )
                return generator.Generate(schema, typeNameHint, reservedTypeNames);
            return spec.TypeNamesMapping[typeNameHint];
        }
    }
}