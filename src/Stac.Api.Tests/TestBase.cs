using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Schema;
using Stac.Schemas;
using Xunit.Abstractions;

namespace Stac.Api.Tests
{
    public abstract class TestBase
    {
        private static readonly Assembly ThisAssembly = typeof(TestBase)
#if NETCOREAPP1_1
        .GetTypeInfo()
#endif
        .Assembly;
        private static readonly string AssemblyName = ThisAssembly.GetName().Name;

        private static StacValidator stacValidator = new StacValidator(new JSchemaUrlResolver());
        protected ITestOutputHelper OutputHelper;

        protected TestBase(ITestOutputHelper outputHelper)
        {
            // Route output from the fixture's logs to xunit's output
            OutputHelper = outputHelper;
        }

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = ThisAssembly.CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        protected string GetJson(string folder, [CallerMemberName] string name = null)
        {
            var type = GetType().Name;
            var path = Path.Combine(AssemblyDirectory, @"../../..", "Resources", folder, type + "_" + name + ".json");

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("file not found at " + path);
            }

            return File.ReadAllText(path);
        }

        protected Uri GetUri(string folder, [CallerMemberName] string name = null)
        {
            var type = GetType().Name;
            var path = Path.Combine(AssemblyDirectory, @"../../..", "Resources", folder, type + "_" + name + ".json");

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("file not found at " + path);
            }

            return new Uri(path);
        }

        protected Uri GetUseCaseFileUri(string name)
        {
            var type = GetType().Name;
            var path = Path.Combine(AssemblyDirectory, @"../../..", "Resources/UseCases", type, name);

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("file not found at " + path);
            }

            return new Uri(path);
        }

        protected string GetUseCaseJson(string name)
        {
            var type = GetType().Name;
            var path = Path.Combine(AssemblyDirectory, @"../../..", "Resources/UseCases", type, name);

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("file not found at " + path);
            }

            return File.ReadAllText(path);
        }

        protected string GetTestCatalogsRootPath()
        {
            var path = Path.Combine(AssemblyDirectory, @"../../..", "Resources/TestCatalogs");

            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException("Directory not found at " + path);
            }

            return path;
        }

        public bool ValidateJson(string jsonstr)
        {
            return stacValidator.ValidateJson(jsonstr);
        }

    }
}
