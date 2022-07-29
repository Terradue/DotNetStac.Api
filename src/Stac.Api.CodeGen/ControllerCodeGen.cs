using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
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
            var document = await OpenApiYamlDocument.FromUrlAsync(url);

            if ( excludedPaths != null )
            {
                foreach (var path in excludedPaths)
                {
                    document.Paths.Remove(path);
                }
            }

            var generator = new CSharpControllerGenerator(document, settings);

            return generator.GenerateFile();
        }
    }
}