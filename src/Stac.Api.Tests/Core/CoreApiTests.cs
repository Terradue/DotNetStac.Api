using Xunit;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Stac.Api.Models;
using Newtonsoft.Json;
using Stac.Api.Clients.Core;
using System;
using System.Linq;

namespace Stac.Api.Tests.Core
{
    [Collection(StacApiAppCollection.Name)]
    public class CoreApiTests : AppTestBase
    {
        public CoreApiTests(StacApiAppFixture fixture, ITestOutputHelper outputHelper) : base(fixture, outputHelper)
        {

        }

        [Fact]
        public void DeserializeLandingPageAsync(){
            LandingPage lp = new LandingPage("sentinel", "Copernicus Sentinel Imagery");
            lp.ConformanceClasses.Add("https://api.stacspec.org/v1.0.0-rc.1/core");
            string json = JsonConvert.SerializeObject(lp);
            ValidateJson(json);
            var catalog = JsonConvert.DeserializeObject<StacCatalog>(json);
            LandingPage lp2 = JsonConvert.DeserializeObject<LandingPage>(json);
        }

        [Theory, MemberData("TestCatalogs", DisableDiscoveryEnumeration = true)]
        public async Task GetLandingPageAsync(string key, string datadir)
        {
            await using var application = new CoreStacApiApplication(datadir);

            var client = application.CreateClient();
            CoreClient coreClient = new CoreClient(client);

            var landingPage = await coreClient.GetLandingPageAsync();

            ValidateLandingPage(landingPage);

            var landingPageJson = StacConvert.Serialize(landingPage);

            ValidateJson(landingPageJson);

            // Assert.Empty(todos);
        }

        private void ValidateLandingPage(LandingPage landingPage)
        {
            Assert.Contains("https://api.stacspec.org/v1.0.0-rc.2/core", landingPage.ConformanceClasses);
            Assert.Contains("https://api.stacspec.org/v1.0.0-rc.2/browseable", landingPage.ConformanceClasses);
            Assert.NotNull(landingPage.Links.FirstOrDefault(l => l.RelationshipType == "self"));
            Assert.NotNull(landingPage.Links.FirstOrDefault(l => l.RelationshipType == "root"));
        }
    }
}