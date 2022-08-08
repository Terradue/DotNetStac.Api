

using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Stac.Api.CodeGen
{
    internal abstract class BaseOperation
    {
        [Option]
        public static bool Verbose { get; set; }

        [Option]
        public static bool Quiet { get; set; }

        [Option("-conf|--config-file", "Config file to use", CommandOptionType.MultipleValue)]
        public string[] ConfigFiles { get; set; }

        [Option("-k|--skip-certificate-validation", "Skip SSL certificate verfification for endpoints", CommandOptionType.NoValue)]
        public bool SkipSsl { get; set; }

        protected static ConsoleReporter logger;

        protected IConsole _console;

        public BaseOperation(IConsole console)
        {
            this._console = console;
        }

        protected IServiceProvider ServiceProvider { get; private set; }

        public static IConfigurationRoot Configuration { get; private set; }

        public ValidationResult OnValidate()
        {
            var serviceCollection = RegisterServices();
            // Build the service provider
            ServiceProvider = serviceCollection.BuildServiceProvider();

            return ValidationResult.Success;
        }

        protected string GetBasePath()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        public async Task<int> OnExecuteAsync()
        {
            try
            {
                if (SkipSsl)
                {
                    ServicePointManager
                        .ServerCertificateValidationCallback +=
                        (sender, cert, chain, sslPolicyErrors) => true;
                }
                await ExecuteAsync();
            }
            catch (CommandParsingException cpe)
            {
                return Program.PrintErrorAndUsage(cpe.Command, cpe.Message);
            }
            finally
            {
                DisposeServices(ServiceProvider);
            }
            return 0;
        }

        protected abstract Task ExecuteAsync();


        private IServiceCollection RegisterServices()
        {

            logger = new ConsoleReporter(_console, Verbose, Quiet);

            var devEnvironmentVariable = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");
            //Determines the working environment as IHostingEnvironment is unavailable in a console app
            var isDevelopment = string.IsNullOrEmpty(devEnvironmentVariable) ||
                                devEnvironmentVariable.ToLower() == "development";

            IServiceCollection collection = new ServiceCollection();

            // Add Configuration
            var builder = new ConfigurationBuilder();
            // tell the builder to look for the appsettings.json file
            builder.AddNewtonsoftJsonFile(Path.Join(GetBasePath(), "../../..", "appsettings.json"), optional: false);

            Configuration = builder.Build();

            collection.AddSingleton<IConfigurationRoot>(Configuration);
            collection.AddOptions();

            // Add the command line services
            collection.AddSingleton<IConsole>(_console);
            collection.AddSingleton<ConsoleReporter>(logger);

            collection.Configure<CodeGenOptions>(Configuration.GetSection("CodeGen"));

            // Registers services specific to operation
            RegisterOperationServices(collection);

            return collection;

        }

        protected abstract void RegisterOperationServices(IServiceCollection collection);

        private static void DisposeServices(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                return;
            }

            if (serviceProvider is IDisposable)
            {
                ((IDisposable)serviceProvider).Dispose();
            }

        }

    }
}