using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Xunit.Abstractions;

namespace Stac.Api.Tests
{
    internal class TestCatalogsProvider : TestBase
    {
        public TestCatalogsProvider() : base(null)
        {
        }

        public IEnumerable<object[]> GetStacApiApplications()
        {
            foreach (var subdir in Directory.GetDirectories(GetTestCatalogsRootPath(), "Catalog*", new EnumerationOptions() { RecurseSubdirectories = false }))
            {
                yield return new object[] { new StacApiApplication(subdir) };
            }
        }

        internal IEnumerable<object[]> GetStacApiApplicationsAndTestDatasets()
        {
            foreach(var app in GetStacApiApplications())
            {
                foreach(var dataset in GetTestCollections())
                {
                    yield return new object[] { app[0], dataset[0], dataset[1] };
                }
            }
        }

        private IEnumerable<object[]> GetTestCollections()
        {
            foreach (var subdir in Directory.GetDirectories(GetTestCollectionsRootPath(), "Collection*", new EnumerationOptions() { RecurseSubdirectories = false }))
            {
                var items = Directory.GetFiles(subdir, "*.json", new EnumerationOptions() { RecurseSubdirectories = true });
                yield return new object[] { subdir + ".json", items };
            }
        }
    }
}