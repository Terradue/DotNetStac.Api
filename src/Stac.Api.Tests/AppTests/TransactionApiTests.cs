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
using Stac.Api.Models.Core;

namespace Stac.Api.Tests.AppTests
{
    [Collection(StacApiAppCollectionFixture.Name)]
    public class TransactionApiTests : AppTestBase
    {
        public TransactionApiTests(ITestOutputHelper outputHelper,
                                   TestCatalogsProvider testCatalogsProvider) : base(outputHelper)
        {
            TestCatalogsProvider = testCatalogsProvider;
            TestCatalogsProvider.SetOutputHelper(outputHelper);
        }

        public TestCatalogsProvider TestCatalogsProvider { get; }

        [Fact]
        public void DeserializeStacCollections()
        {
            StacCollections colls = new StacCollections();
            string json = JsonConvert.SerializeObject(colls);
            var catalog = JsonConvert.DeserializeObject<StacCollections>(json);
        }

        [Theory, MemberData(nameof(GetTestDatasets))]
        public async Task PostAllCollectionItemsAsync(string collectionPath, string[] itemsPaths)
        {
            StacApiApplication application = TestCatalogsProvider.CreateTemporaryCatalog(nameof(PostAllCollectionItemsAsync));
            var client = application.CreateClient();
            TransactionClient transactionClient = new TransactionClient(client);
            FeaturesClient featuresClient = new FeaturesClient(client);
            CollectionsClient collectionsClient = new CollectionsClient(client);

            var collectionJson = File.ReadAllText(collectionPath);
            StacCollection collection = StacConvert.Deserialize<StacCollection>(collectionJson);

            int totalItemsInCollection = 0;

            foreach (var itemPath in itemsPaths)
            {
                var itemJson = File.ReadAllText(itemPath);
                var post = JsonConvert.DeserializeObject<PostStacItemOrCollection>(itemJson);
                var result = await transactionClient.PostFeatureAsync(post, collection.Id);
                if (post.IsCollection)
                {
                    totalItemsInCollection += post.StacFeatureCollection.Features.Count();
                    StacFeatureCollection stacItems = await featuresClient.GetFeaturesAsync(collection.Id, 100, null, null);
                    int countItemsInCollection = 0;
                    while (true)
                    {
                        countItemsInCollection += stacItems.Features.Count();
                        if (stacItems.NextPage() == null)
                        {
                            break;
                        }
                        var json = await client.GetStringAsync(stacItems.NextPage().Uri);
                        stacItems = JsonConvert.DeserializeObject<StacFeatureCollection>(json);
                    }
                    Assert.Equal(totalItemsInCollection, countItemsInCollection);
                }
                else
                {
                    totalItemsInCollection++;
                    var result2 = await featuresClient.GetFeatureAsync(collection.Id, result.Id);
                    var resultJson = JsonConvert.SerializeObject(result);
                    var result2Json = JsonConvert.SerializeObject(result2);
                    JsonAssert.AreEqual(resultJson, result2Json);
                    result.Links.Clear();
                    resultJson = JsonConvert.SerializeObject(result);
                    JsonAssert.AreEqual(itemJson, resultJson);
                }
            }

            // var collection2 = await collectionsClient.DescribeCollectionAsync(collection.Id);
            // var collection2Json = StacConvert.Serialize(collection2);
            // When collection edit API ready, uncomment this line
            //JsonAssert.AreEqual(collectionJson, collection2Json);
        }

        [Theory, MemberData(nameof(GetTestDatasets))]
        public async Task PostAllCollectionItemsAgainAsync(string collectionPath, string[] itemsPaths)
        {
            StacApiApplication application = TestCatalogsProvider.CreateTemporaryCatalog(nameof(PostAllCollectionItemsAsync));
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
                var result = await transactionClient.PostFeatureAsync(post, collection.Id + "again");
                // Check that the exception is a 409 Conflict
                try
                {
                    await transactionClient.PostFeatureAsync(post, collection.Id + "again");
                }
                catch (Stac.Api.StacApiException e)
                {
                    Assert.Equal(409, e.StatusCode);
                }
            }

        }
    }
}
