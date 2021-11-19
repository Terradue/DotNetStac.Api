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
    internal class ClientCodeGen
    {
        private readonly IOptions<CodeGenOptions> options;

        public ClientCodeGen(IOptions<CodeGenOptions> options)
        {
            this.options = options;
        }

        public async Task ExecuteAsync(string generatedCodeBasePath)
        {
            foreach (var spec in options.Value.Specifications)
            {
                string code = await GenerateCodeFromUrl(spec.Value.Url, options.Value.GenerateSettings(spec.Key));
                string path = Path.Join(generatedCodeBasePath, spec.Value.OutputFilePath);
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                File.WriteAllText(path, code);
            }
        }

        private async Task<string> GenerateCodeFromUrl(string url, CSharpClientGeneratorSettings settings)
        {
            var document = await OpenApiYamlDocument.FromUrlAsync(url);

            var generator = new CSharpClientGenerator(document, settings);

            return generator.GenerateFile();
        }
    }
}