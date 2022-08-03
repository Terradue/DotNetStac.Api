using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NJsonSchema;
using NJsonSchema.Yaml;
using NSwag;
using NSwag.CodeGeneration.CSharp;

namespace Stac.Api.CodeGen
{
    internal class ControllerCodeGen
    {
        private readonly IOptions<CodeGenOptions> options;

        public ControllerCodeGen(IOptions<CodeGenOptions> options)
        {
            this.options = options;
        }

        public async Task ExecuteAsync(string generatedCodeBasePath)
        {
            foreach (var spec in options.Value.Specifications)
            {
                string code = await GenerateCodeFromUrl(spec.Value.Url, options.Value.GenerateControllerGeneratorSettings(spec.Key), spec.Value.ExcludedPaths);
                string path = Path.Join(generatedCodeBasePath, spec.Value.ControllerOutputFilePath);
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                File.WriteAllText(path, code);
            }
        }

        private async Task<string> GenerateCodeFromUrl(string url, CSharpControllerGeneratorSettings settings, IEnumerable<string> excludedPaths)
        {
            // HttpClient httpClient = new HttpClient();
            // var yaml = await httpClient.GetStringAsync(url);
            
            // var document = await OpenApiYamlDocument.FromYamlAsync(yaml, null, SchemaType.OpenApi3, ResolveSchema);

            var document = await OpenApiYamlDocument.FromUrlAsync(url);

            if ( excludedPaths != null )
            {
                foreach (var path in excludedPaths)
                {
                    document.Paths.Remove(path);
                }
            }

            FilterDocument(document);

            var generator = new CSharpControllerGenerator(document, settings);

            return generator.GenerateFile();
        }

        private JsonReferenceResolver ResolveSchema(OpenApiDocument arg)
        {
            var factory = JsonAndYamlReferenceResolver.CreateJsonAndYamlReferenceResolverFactory(new DefaultTypeNameGenerator());
            return factory(null);
        }

        private void FilterDocument(OpenApiDocument document)
        {
            // Fix unsupported array for bbox (https://github.com/radiantearth/stac-api-spec/blob/v1.0.0-rc.1/item-search/openapi.yaml#L179)
            // Replace by a simple string
            if (document.Components.Parameters.ContainsKey("bbox")){
                var bboxParam = document.Components.Parameters["bbox"];
                if ( bboxParam.Schema.Type == JsonObjectType.Array )
                {
                    bboxParam.Schema.Type = JsonObjectType.String;
                    bboxParam.Schema.OneOf.Clear();
                    bboxParam.Schema.Items.Clear();
                }
            }
        }
    }
}