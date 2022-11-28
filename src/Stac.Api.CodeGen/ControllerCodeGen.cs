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
    internal class ControllerCodeGen : BaseCodeGen
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
                    try
                    {
                        content = await client.GetStringAsync(spec.Value.Url);
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Unable to download OpenAPI specification from {spec.Value.Url}", e);
                    }
                    documentPath = spec.Value.Url;
                }
                else if (!string.IsNullOrEmpty(spec.Value.File))
                {
                    content = await File.ReadAllTextAsync(spec.Value.File);
                    documentPath = spec.Value.File;
                }
                OpenApiDocument document = await OpenApiYamlDocument.FromYamlAsync(content, documentPath, SchemaType.OpenApi3, doc => GetResolver(doc, spec.Value.ExcludedSchemas));
                // JsonSchemaReferenceUtilities.UpdateSchemaReferencePaths(document, true, new DefaultContractResolver());
                string code = await GenerateCode(document, options.Value.GenerateControllerGeneratorSettings(spec.Key), spec.Value.ExcludedOperations);
                string path = Path.Join(generatedCodeBasePath, spec.Value.ControllerOutputFilePath);
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                File.WriteAllText(path, code);
            }
        }

        private async Task<string> GenerateCode(OpenApiDocument document, CSharpControllerGeneratorSettings settings, IEnumerable<string> excludedOperations)
        {
            if (excludedOperations != null)
            {
                foreach (var path in excludedOperations)
                {
                    var operation = path.Split(' ');
                    document.Paths[operation[1]].Remove(operation[0].ToLower());
                }
            }

            FilterDocument(document);

            var generator = new CSharpControllerGenerator(document, settings);

            return generator.GenerateFile();
        }


    }
}