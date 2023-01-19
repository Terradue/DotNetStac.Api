using Xunit;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Stac.Api.Models;
using Newtonsoft.Json;
using Stac.Api.Clients.Collections;
using System.Linq;
using Stac.Api.Clients.Extensions;
using System.IO;
using Stac.Api.Clients.Features;

namespace Stac.Api.Tests.AppTests
{
    [Collection(StacApiAppCollectionFixture.Name)]
    public class TransactionApiTests : AppTestBase
    {
        public TransactionApiTests(StacApiAppFixture fixture, ITestOutputHelper outputHelper) : base(fixture, outputHelper)
        {
            
        }

        [Fact]
        public void DeserializeStacCollections(){
            StacCollections colls = new StacCollections();
            string json = JsonConvert.SerializeObject(colls);
            var catalog = JsonConvert.DeserializeObject<StacCollections>(json);
        }

        [Theory, MemberData(nameof(TestCatalogs), new object[] { nameof(TransactionApiTests) })]
        public async Task PostAllCollectionItemsAsync(StacApiApplication application, string collectionPath, string[] itemsPaths)
        {
            var client = application.CreateClient();
            TransactionClient transactionClient = new TransactionClient(client);
            FeaturesClient featuresClient = new FeaturesClient(client);
            CollectionsClient collectionsClient = new CollectionsClient(client);

            var collectionJson = File.ReadAllText(collectionPath);
            StacCollection collection = StacConvert.Deserialize<StacCollection>(collectionJson);

            foreach (var itemPath in itemsPaths)
            {
                var itemJson = File.ReadAllText(itemPath);
                var post = JsonConvert.DeserializeObject<PostStacItemOrCollection>(itemJson);
                var result = await transactionClient.PostFeatureAsync(post, collection.Id);
                var result2 = await featuresClient.GetFeatureAsync(collection.Id, result.Id);
                var resultJson = JsonConvert.SerializeObject(result);
                var result2Json = JsonConvert.SerializeObject(result2);
                JsonAssert.AreEqual(resultJson, result2Json);
                result.Links.Clear();
                resultJson = JsonConvert.SerializeObject(result);
                JsonAssert.AreEqual(itemJson, resultJson);
            }
            
            var collection2 = await collectionsClient.DescribeCollectionAsync(collection.Id);
            var collection2Json = StacConvert.Serialize(collection2);
            // When collection edit API ready, uncomment this line
            //JsonAssert.AreEqual(collectionJson, collection2Json);
        }
    }
}
