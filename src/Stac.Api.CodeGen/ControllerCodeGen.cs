using System;
using System.IO;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
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
                string code = await GenerateCodeFromUrl(spec.Value.Url, options.Value.GenerateControllerGeneratorSettings(spec.Key));
                string path = Path.Join(generatedCodeBasePath, spec.Value.ControllerOutputFilePath);
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                File.WriteAllText(path, code);
            }
        }

        private async Task<string> GenerateCodeFromUrl(string url, CSharpControllerGeneratorSettings settings)
        {
            var document = await OpenApiYamlDocument.FromUrlAsync(url);

            var generator = new CSharpControllerGenerator(document, settings);

            return generator.GenerateFile();
        }
    }
}