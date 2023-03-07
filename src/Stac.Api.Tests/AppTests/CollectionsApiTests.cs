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
        public void DeserializeStacCollections()
        {
            StacCollections colls = new StacCollections();
            string json = JsonConvert.SerializeObject(colls);
            var catalog = JsonConvert.DeserializeObject<StacCollections>(json);
        }

        [Theory, MemberData(nameof(GetTestCatalogs), new object[] { new string[] { "Catalog1" } })]
        public async Task GetCollectionsAsync(StacApiApplication application)
        {
            var client = application.CreateClient();
            CollectionsClient collectionsClient = new CollectionsClient(client);

            StacCollections collections = await collectionsClient.GetCollectionsAsync();

            Assert.Equal(1, collections.Collections.Count);
            Assert.Equal("test", collections.Collections.First().Id);

            ValidateCollections(collections);

            foreach (var collection in collections.Collections)
            {
                string collectionJson = JsonConvert.SerializeObject(collection);
                ValidateJson(collectionJson);
                var collection2 = await collectionsClient.DescribeCollectionAsync(collection.Id);
                var collection2Json = JsonConvert.SerializeObject(collection2);
                JsonAssert.AreEqual(collectionJson, collection2Json);
                ValidateJson(collection2Json);
            }

        }

        [Theory, MemberData(nameof(GetTestCatalogs), new object[] { new string[] { "Catalog1" } })]
        public async Task GetCollectionsByIdAsync(StacApiApplication application)
        {
            var client = application.CreateClient();
            CollectionsClient collectionsClient = new CollectionsClient(client);

            StacCollection testCollection = await collectionsClient.DescribeCollectionAsync("test");

            Assert.Equal("test", testCollection.Id);

            string collectionJson = JsonConvert.SerializeObject(testCollection);
            ValidateJson(collectionJson);

        }

        private void ValidateCollections(StacCollections collections)
        {
            Assert.NotNull(collections.Links.FirstOrDefault(l => l.RelationshipType == "self"));
            Assert.NotNull(collections.Links.FirstOrDefault(l => l.RelationshipType == "root"));
        }
    }
}
