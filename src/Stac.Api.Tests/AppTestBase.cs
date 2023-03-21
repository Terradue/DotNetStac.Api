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
    public abstract class AppTestBase : TestBase, IDisposable
    {
        protected static TestCatalogsProvider _testCatalogsProvider;

        protected AppTestBase(ITestOutputHelper outputHelper) : base(outputHelper)
        {
        }

        public static IEnumerable<object[]> GetTestCatalogs(string[] catalogNames)
        {
            return TestCatalogsProvider.GetStacApiApplications(catalogNames);
        }

        public static IEnumerable<object[]> GetTestDatasets()
        {
            return TestCatalogsProvider.GetTestDatasets();
        }

        protected static TestCatalogsProvider TestCatalogsProvider
        {
            get
            {
                if (_testCatalogsProvider == null)
                {
                    _testCatalogsProvider = new TestCatalogsProvider();
                    _testCatalogsProvider.Init();
                }
                return _testCatalogsProvider;
            }
        }

        public void Dispose()
        {
            if (_testCatalogsProvider != null)
            {
                _testCatalogsProvider.Dispose();
                _testCatalogsProvider = null;
            }
        }
    }
}
