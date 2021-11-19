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
    [Command(Name = "client", Description = "Generate the Client Code for the OpenApi Spec")]
    internal class ClientCodeGenerator : BaseOperation
    {
        public ClientCodeGenerator(IConsole console) : base(console)
        {
        }

        protected override async Task ExecuteAsync()
        {
            ClientCodeGen clientCodeGen = ServiceProvider.GetService<ClientCodeGen>();
            await clientCodeGen.ExecuteAsync(Path.Join(GetBasePath(), "../../../../"));
        }

        protected override void RegisterOperationServices(IServiceCollection collection)
        {
            collection.AddTransient<ClientCodeGen>();
        }


    }
}