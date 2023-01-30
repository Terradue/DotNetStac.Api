using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Xunit.Abstractions;

namespace Stac.Api.Tests
{
    public class TestCatalogsProvider : TestBase, IDisposable
    {
        public TestCatalogsProvider() : base(null)
        {
        }

        public void Init()
        {
            foreach (var subdir in GetReferenceCatalogsDirectories())
            {
                var tempDir = Path.Combine(GetTempCatalogsPath(), Path.GetRelativePath(GetTestCatalogsRootPath(), subdir));
                if (Directory.Exists(tempDir))
                {
                    Directory.Delete(tempDir, true);
                }
                Directory.CreateDirectory(tempDir);
                // Copy directory
                foreach (var file in Directory.GetFiles(subdir, "*", new EnumerationOptions() { RecurseSubdirectories = true }))
                {
                    var destFile = Path.Combine(tempDir, Path.GetRelativePath(subdir, file));
                    Directory.CreateDirectory(Path.GetDirectoryName(destFile));
                    File.Copy(file, destFile);
                }
            }
        }

        private string GetTempCatalogsPath()
        {
            return Path.Combine(Path.GetTempPath(), "StacApiTests", "Catalogs");
        }

        public IEnumerable<object[]> GetStacApiApplications()
        {
            foreach (var subdir in GetTempCatalogsDirectories())
            {
                yield return new object[] { new StacApiApplication(subdir) };
            }
        }

        public IEnumerable<object[]> GetStacApiApplications(string[] catalogNames)
        {
            foreach (var name in catalogNames)
            {
                yield return new object[] { new StacApiApplication(Path.Combine(GetTestCatalogsRootPath(), "Catalog*")) };
            }
        }

        private string[] GetReferenceCatalogsDirectories(string searchPattern = "Catalog*")
        {
            return Directory.GetDirectories(GetTestCatalogsRootPath(), "Catalog*", new EnumerationOptions() { RecurseSubdirectories = false });
        }

        private string[] GetTempCatalogsDirectories()
        {
            return Directory.GetDirectories(GetTempCatalogsPath(), "Catalog*", new EnumerationOptions() { RecurseSubdirectories = false });
        }

        internal IEnumerable<object[]> GetStacApiApplicationsAndTestDatasets()
        {
            foreach (var app in GetStacApiApplications())
            {
                foreach (var dataset in GetTestCollections())
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

        public void Dispose()
        {
            if (Directory.Exists(GetTempCatalogsPath()))
            {
                Directory.Delete(GetTempCatalogsPath(), true);
            }
        }
    }
}