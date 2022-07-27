using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using McMaster.Extensions.CommandLineUtils;

namespace Stac.Api.CodeGen
{
    [Command(Name = "StacApiGen", FullName = "STAC API Code Generator", Description = "Spatio Temporal Asset Catalog API C# Code Generator")]
    [HelpOption]
    [Subcommand(
        typeof(ClientCodeGenerator),
        typeof(ControllerCodeGenerator)
    )]
    class Program
    {
        public static int Main(string[] args)
        {
            CommandLineApplication<Program> app = CreateApplication(PhysicalConsole.Singleton);

            try
            {
                return app.Execute(args);
            }
            catch (CommandParsingException cpe)
            {
                return PrintErrorAndUsage(cpe.Command, cpe.Message);
            }
            catch (TargetInvocationException e)
            {
                PhysicalConsole.Singleton.Error.WriteLine(e.InnerException.Message);
                PhysicalConsole.Singleton.Error.WriteLine(e.InnerException.StackTrace);
                return 1;
            }
            catch (Exception e)
            {
                PhysicalConsole.Singleton.Error.WriteLine(e.Message);
                PhysicalConsole.Singleton.Error.WriteLine(e.StackTrace);
                return 1;
            }
        }

        public static CommandLineApplication<Program> CreateApplication(IConsole console)
        {
            var app = new CommandLineApplication<Program>(console);
            app.VersionOptionFromAssemblyAttributes(typeof(Program).Assembly);
            app.Conventions.UseDefaultConventions();
            return app;
        }

        public static int PrintErrorAndUsage(CommandLineApplication command, string message)
        {
            PhysicalConsole.Singleton.Error.WriteLineAsync("Parsing Error: " + message);
            command.ShowHelp();
            return 2;
        }

        private async void OnExecuteAsync(CommandLineApplication app)
        {
            await PhysicalConsole.Singleton.Error.WriteLineAsync("Specify a subcommand");
            app.ShowHelp();

        }

        public static int OnValidationError(CommandLineApplication command, ValidationResult ve)
        {
            PhysicalConsole.Singleton.Error.WriteLine(ve.ErrorMessage);
            command.ShowHelp();
            return 1;
        }
    }
}
