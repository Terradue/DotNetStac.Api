using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NJsonSchema;
using NSwag;
using NSwag.CodeGeneration.CSharp;

namespace Stac.Api.CodeGen
{
    internal class ClientCodeGen : BaseCodeGen
    {
        private readonly IOptions<CodeGenOptions> options;

        public ClientCodeGen(IOptions<CodeGenOptions> options)
        {
            this.options = options;
        }

        public async Task ExecuteAsync(string generatedCodeBasePath)
        {
            foreach (var spec in options.Value.Specifications)
            {string content = null;
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
                string code = await GenerateCode(document, options.Value.GenerateClientGeneratorSettings(spec.Key), spec.Value.ExcludedPaths);
                string path = Path.Join(generatedCodeBasePath, spec.Value.ClientOutputFilePath);
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                File.WriteAllText(path, code);
            }
        }

        private async Task<string> GenerateCode(OpenApiDocument document, CSharpClientGeneratorSettings settings, IEnumerable<string> excludedPaths)
        {
            if (excludedPaths != null)
            {
                foreach (var path in excludedPaths)
                {
                    document.Paths.Remove(path);
                }
            }

            var generator = new CSharpClientGenerator(document, settings);

            FilterDocument(document);

            return generator.GenerateFile();
        }
    }
}