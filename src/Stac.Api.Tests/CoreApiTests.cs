using System;
using Xunit;
using System.Threading.Tasks;
using Stac.Api.Clients;
using Xunit.Abstractions;
using Stac.Api.Models;
using Newtonsoft.Json;

namespace Stac.Api.Tests
{
    [Collection(StacApiAppCollection.Name)]
    public class CoreApiTests : TestBase
    {
        public CoreApiTests(StacApiAppFixture fixture, ITestOutputHelper outputHelper) : base(fixture, outputHelper)
        {

        }

        [Fact]
        public void DeserializeLandingPageAsync(){
            LandingPage lp = new LandingPage("test", "test");
            lp.ConformanceClasses.Add("https://api.stacspec.org/v1.0.0-rc.1/core");
            string json = JsonConvert.SerializeObject(lp);
            ValidateJson(json);
            var catalog = JsonConvert.DeserializeObject<StacCatalog>(json);
            LandingPage lp2 = JsonConvert.DeserializeObject<LandingPage>(json);
        }

        [Fact]
        public async Task GetLandingPageAsync()
        {
            await using var application = new CoreStacApiApplication();

            var client = application.CreateClient();
            CoreClient coreClient = new CoreClient("https://localhost", client);

            var landingPage = await coreClient.GetLandingPageAsync();

            var landingPageJson = StacConvert.Serialize(landingPage);

            ValidateJson(landingPageJson);

            // Assert.Empty(todos);
        }
    }
}
