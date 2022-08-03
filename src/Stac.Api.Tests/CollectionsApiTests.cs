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
    public class CollectionsApiTests : AppTestBase
    {
        public CollectionsApiTests(StacApiAppFixture fixture, ITestOutputHelper outputHelper) : base(fixture, outputHelper)
        {

        }

        [Fact]
        public async Task GetCollectionsAsync()
        {
            await using var application = new CollectionsStacApiApplication();

            var client = application.CreateClient();
            CollectionsClient collectionsClient = new CollectionsClient("https://localhost", client);

            var landingPage = await collectionsClient.GetLandingPageAsync();

            var landingPageJson = StacConvert.Serialize(landingPage);

            ValidateJson(landingPageJson);

            var collections = await collectionsClient.GetCollectionsAsync();

            foreach(var collection in collections.Collections1){
                var collectionJson = StacConvert.Serialize(collection);
                ValidateJson(collectionJson);
                var collectionById = await collectionsClient.DescribeCollectionAsync(collection.Id);
                var collectionByIdJson = StacConvert.Serialize(collectionById);
                ValidateJson(collectionByIdJson);
                JsonAssert.AreEqual(collectionJson, collectionByIdJson);
            }
        }
    }
}
