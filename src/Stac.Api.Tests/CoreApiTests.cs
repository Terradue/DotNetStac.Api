using System;
using Xunit;
using Stac.Api.Generated.Clients;
using System.Threading.Tasks;

namespace Stac.Api.Tests
{
    [Collection(StacApiAppCollection.Name)]
    public class CoreApiTests : TestBase
    {
        [Fact]
        public async Task GetLandingPageAsync()
        {
            await using var application = new CoreStacApiApplication();

            var client = application.CreateClient();
            CoreClient coreClient = new CoreClient("dummy", client);

            var landingPage = await coreClient.GetLandingPageAsync();

            var landingPageJson = StacConvert.Serialize(landingPage);

            ValidateJson(landingPageJson);

            // Assert.Empty(todos);
        }
    }
}
