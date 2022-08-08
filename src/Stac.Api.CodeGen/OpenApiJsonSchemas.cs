using NJsonSchema.References;
using NSwag;

namespace Stac.Api.CodeGen
{
    internal class OpenApiJsonSchemas : JsonReferenceBase<OpenApiJsonSchemas>, IJsonReference
    {
        private OpenApiDocument _openapi;

        public OpenApiJsonSchemas(OpenApiDocument openapi, string documentPath)
        {
            _openapi = openapi;
            DocumentPath = documentPath;
        }

        public IJsonReference ActualObject => this;

        public object PossibleRoot => _openapi;

        public string ReferencePath { get => null; set {} }

        public OpenApiDocument OpenApi => _openapi;
    }
}