using System.IO;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace Stac.Api.CodeGen
{
    [Command(Name = "controller", Description = "Generate the Web API Code for the OpenApi Spec")]
    internal class ControllerCodeGenerator : BaseOperation
    {
        public ControllerCodeGenerator(IConsole console) : base(console)
        {
        }

        protected override async Task ExecuteAsync()
        {
            ControllerCodeGen clientCodeGen = ServiceProvider.GetService<ControllerCodeGen>();
            await clientCodeGen.ExecuteAsync(Path.Join(GetBasePath(), "../../../../"));
        }

        protected override void RegisterOperationServices(IServiceCollection collection)
        {
            collection.AddTransient<ControllerCodeGen>();
        }


    }
}