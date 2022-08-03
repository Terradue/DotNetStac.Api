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
    public abstract class AppTestBase: TestBase
    {
        private StacApiAppFixture Fixture;

        protected AppTestBase(StacApiAppFixture fixture, ITestOutputHelper outputHelper): base(outputHelper)
        {
            Fixture = fixture;
            // Route output from the fixture's logs to xunit's output
            Fixture.SetOutputHelper(OutputHelper);
        }


    }
}
