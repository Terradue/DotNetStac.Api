using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using MartinCostello.Logging.XUnit;
using Xunit.Abstractions;

namespace Stac.Api.Tests
{
    public class TestCatalogsProvider : IDisposable
    {
        private ITestOutputHelper _outputHelper;

        public TestCatalogsProvider()
        {
            Init();
        }

        public void SetOutputHelper(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
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
                yield return new object[] { new StacApiApplication(subdir, _outputHelper) };
            }
        }

        public IEnumerable<object[]> GetStacApiApplications(string[] catalogNames)
        {
            foreach (var name in catalogNames)
            {
                yield return new object[] { GetStacApiApplication(name) };
            }
        }

        public StacApiApplication GetStacApiApplication(string name)
        {
            return new StacApiApplication(Path.Combine(GetTestCatalogsRootPath(), name), _outputHelper);
        }


        public StacApiApplication CreateTemporaryCatalog(string catalogName)
        {
            var tempDir = Path.Combine(GetTempCatalogsPath(), catalogName);
            if (!Directory.Exists(tempDir))
            {
                Directory.CreateDirectory(tempDir);
            }
            return new StacApiApplication(tempDir, _outputHelper);
        }

        private string[] GetReferenceCatalogsDirectories(string searchPattern = "Catalog*")
        {
            return Directory.GetDirectories(GetTestCatalogsRootPath(), "Catalog*", new EnumerationOptions() { RecurseSubdirectories = false });
        }

        private string[] GetTempCatalogsDirectories()
        {
            return Directory.GetDirectories(GetTempCatalogsPath(), "Catalog*", new EnumerationOptions() { RecurseSubdirectories = false });
        }

        

        protected string GetTestCatalogsRootPath()
        {
            var path = Path.Combine(TestBase.AssemblyDirectory, @"../../..", "Resources/TestCatalogs");

            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException("Directory not found at " + path);
            }

            return path;
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