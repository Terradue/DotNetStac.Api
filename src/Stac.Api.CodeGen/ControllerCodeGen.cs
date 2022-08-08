using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using NJsonSchema;
using NJsonSchema.Generation;
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
                string content = null;
                string documentPath = null;
                if (!string.IsNullOrEmpty(spec.Value.Url))
                {
                    HttpClient client = new HttpClient();
                    content = await client.GetStringAsync(spec.Value.Url);
                    documentPath = spec.Value.Url;
                }
                else if (!string.IsNullOrEmpty(spec.Value.File))
                {
                    content = await File.ReadAllTextAsync(spec.Value.File);
                    documentPath = spec.Value.File;
                }
                OpenApiDocument document = await OpenApiYamlDocument.FromYamlAsync(content, documentPath, SchemaType.OpenApi3, doc => GetResolver(doc, spec.Value.ExcludedSchemas));
                // JsonSchemaReferenceUtilities.UpdateSchemaReferencePaths(document, true, new DefaultContractResolver());
                string code = await GenerateCode(document, options.Value.GenerateControllerGeneratorSettings(spec.Key), spec.Value.ExcludedPaths);
                string path = Path.Join(generatedCodeBasePath, spec.Value.ControllerOutputFilePath);
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                File.WriteAllText(path, code);
            }
        }

        private JsonReferenceResolver GetResolver(OpenApiDocument arg, IEnumerable<string> excludedDefinitions)
        {
            var schemaResolver = new OpenApiSchemaResolver(arg, new JsonSchemaGeneratorSettings());
            return new StacReferenceResolver(schemaResolver, excludedDefinitions);
        }

        private async Task<string> GenerateCode(OpenApiDocument document, CSharpControllerGeneratorSettings settings, IEnumerable<string> excludedPaths)
        {
            if (excludedPaths != null)
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

        private void FilterDocument(OpenApiDocument document)
        {
            // Fix unsupported array for bbox (https://github.com/radiantearth/stac-api-spec/blob/v1.0.0-rc.1/item-search/openapi.yaml#L179)
            // Replace by a simple string
            if (document.Components.Parameters.ContainsKey("bbox"))
            {
                var bboxParam = document.Components.Parameters["bbox"];
                if (bboxParam.Schema.Type == JsonObjectType.Array)
                {
                    bboxParam.Schema.Type = JsonObjectType.String;
                    bboxParam.Schema.OneOf.Clear();
                    bboxParam.Schema.Items.Clear();
                }
            }

            // foreach (var path in document.Paths)
            // {
            //     foreach (var method in path.Value)
            //     {
            //         foreach (var response in method.Value.Responses)
            //         {
            //             if (response.Key == "default")
            //             {
            //                 foreach (var content in response.Value.ActualResponse.Content)
            //                 {
            //                     if (content.Value.Schema != null)
            //                     {
            //                         try
            //                         {
            //                             var schema = content.Value.Schema.ActualSchema;
            //                         }
            //                         catch (Exception e)
            //                         {
            //                             content.Value.Schema = JsonSchema.CreateAnySchema();
            //                         }
            //                     }
            //                 }
            //             }
            //         }
            //     }
            // }

        }
    }
}