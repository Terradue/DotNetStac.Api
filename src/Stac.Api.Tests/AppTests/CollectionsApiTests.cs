using Xunit;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Stac.Api.Models;
using Newtonsoft.Json;
using Stac.Api.Clients.Collections;
using System.Linq;

namespace Stac.Api.Tests.AppTests
{
    [Collection(StacApiAppCollectionFixture.Name)]
    public class CollectionsApiTests : AppTestBase
    {
        public CollectionsApiTests(StacApiAppFixture fixture, ITestOutputHelper outputHelper) : base(fixture, outputHelper)
        {
            
        }

        [Fact]
        public void DeserializeStacCollections(){
            StacCollections colls = new StacCollections();
            string json = JsonConvert.SerializeObject(colls);
            var catalog = JsonConvert.DeserializeObject<StacCollections>(json);
        }

        [Theory, MemberData("TestCatalogs", DisableDiscoveryEnumeration = true)]
        public async Task GetCollectionsAsync(string key, string datadir)
        {
            await using var application = new StacApiApplication(datadir);

            var client = application.CreateClient();
            CollectionsClient collectionsClient = new CollectionsClient(client);

            var collections = await collectionsClient.GetCollectionsAsync();

            ValidateCollections(collections);

            var CollectionsJson = JsonConvert.SerializeObject(collections);

            ValidateJson(CollectionsJson);

        }

        private void ValidateCollections(StacCollections collections)
        {
            Assert.NotNull(collections.Links.FirstOrDefault(l => l.RelationshipType == "self"));
            Assert.NotNull(collections.Links.FirstOrDefault(l => l.RelationshipType == "root"));
        }
    }
}
