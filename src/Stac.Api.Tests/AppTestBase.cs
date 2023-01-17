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
        private StacApiAppFixture Fixture;
        private static TestCatalogsProvider _testCatalogsProvider;

        protected AppTestBase(StacApiAppFixture fixture, ITestOutputHelper outputHelper) : base(outputHelper)
        {
            Fixture = fixture;
            // Route output from the fixture's logs to xunit's output
            Fixture.SetOutputHelper(OutputHelper);
        }

        public static IEnumerable<object[]> TestCatalogs(string testClassName)
        {
            switch (testClassName)
            {
                case nameof(TransactionApiTests):
                    return TestCatalogsProvider.GetStacApiApplicationsAndTestDatasets();
                default:
                    return TestCatalogsProvider.GetStacApiApplications();
            }
        }

        private static TestCatalogsProvider TestCatalogsProvider
        {
            get
            {
                if (_testCatalogsProvider == null)
                {
                    _testCatalogsProvider = new TestCatalogsProvider();
                }
                return _testCatalogsProvider;
            }
        }


    }
}
