using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Schema;
using Stac.Api.Tests.AppTests;
using Stac.Schemas;
using Xunit.Abstractions;

namespace Stac.Api.Tests
{
    public abstract class AppTestBase : TestBase
    {

        protected AppTestBase(ITestOutputHelper outputHelper) : base(outputHelper)
        {
        }

        public static IEnumerable<object[]> GetTestDatasets()
        {
            foreach (var subdir in Directory.GetDirectories(GetTestDatasetsRootPath(), "Collection*", new EnumerationOptions() { RecurseSubdirectories = false }))
            {
                var items = Directory.GetFiles(subdir, "*.json", new EnumerationOptions() { RecurseSubdirectories = true });
                yield return new object[] { subdir + ".json", items };
            }
        }

        protected static string GetTestDatasetsRootPath()
        {
            var path = Path.Combine(TestBase.AssemblyDirectory, @"../../..", "Resources/TestDatasets");

            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException("Directory not found at " + path);
            }

            return path;
        }

       
    }
}
