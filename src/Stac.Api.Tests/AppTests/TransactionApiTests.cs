using Xunit;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Stac.Api.Models;
using Newtonsoft.Json;
using Stac.Api.Clients.Collections;
using System.Linq;
using Stac.Api.Clients.Extensions;
using System.IO;

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
        public async Task PostFeatureAsync(StacApiApplication application, string collectionPath, string[] itemsPaths)
        {
            var client = application.CreateClient();
            TransactionClient transactionClient = new TransactionClient(client);

            var collectionJson = File.ReadAllText(collectionPath);
            StacCollection collection = StacConvert.Deserialize<StacCollection>(collectionJson);

            foreach (var itemPath in itemsPaths)
            {
                var itemJson = File.ReadAllText(itemPath);
                var post = JsonConvert.DeserializeObject<PostStacItemOrCollection>(itemJson);
                var result = await transactionClient.PostFeatureAsync(post, collection.Id);
                var resultJson = JsonConvert.SerializeObject(result);
                JsonAssert.AreEqual(itemJson, resultJson);
            }
            

        }
    }
}
