using Xunit;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Stac.Api.Models;
using Newtonsoft.Json;
using Stac.Api.Clients.Collections;
using System.Linq;
using Stac.Api.Clients.ItemSearch;
using GeoJSON.Net.Geometry;
using Stac.Api.Extensions.Filters;

namespace Stac.Api.Tests.AppTests
{
    [Collection(StacApiAppCollectionFixture.Name)]
    public class ItemSearchApiTests : AppTestBase
    {
        public ItemSearchApiTests(ITestOutputHelper outputHelper) : base(outputHelper)
        {

        }

        [Theory, MemberData(nameof(GetTestCatalogs), new object[] { new string[] { "CatalogS2L2A" } })]
        public async Task GetItemSearchAsync(StacApiApplication application)
        {
            var client = application.CreateClient();
            ItemSearchClient itemSearchClient = new ItemSearchClient(client);

            var polygon = new Polygon(new double[][][]
                {
                    new double[][]
                    {
                        new double[] { 115.6433814, 57.6452553 },
                        new double[] { 115.6540305, 57.6649078 },
                        new double[] { 115.7314869, 57.809336 },
                        new double[] { 115.8109064, 57.9534193 },
                        new double[] { 115.8872017, 58.0983519 },
                        new double[] { 115.967219, 58.2424627 },
                        new double[] { 116.0459009, 58.3869795 },
                        new double[] { 116.1261709, 58.5312986 },
                        new double[] { 116.1846498, 58.6346043 },
                        new double[] { 117.1681312, 58.6405464 },
                        new double[] { 117.1635452, 57.6543437 },
                        new double[] { 115.6433814, 57.6452553 }
                    }
                });

            var result = await itemSearchClient.GetItemSearchAsync(
                null,
                new Stac.Api.Models.Core.IntersectGeometryFilter(polygon),
                null,
                10,
                null,
                new string[] { "sentinel-2-l2a" }
            );

            Assert.NotNull(result);
            Assert.NotNull(result.Features);
            Assert.NotEmpty(result.Features);
            Assert.NotNull(result.Links);
            Assert.NotEmpty(result.Links);
            Assert.Equal(6, result.Features.Count);
            foreach (var feature in result.Features)
            {
                Assert.True(feature.Geometry.Intersects(polygon));
            }

        }

        [Theory, MemberData(nameof(GetTestCatalogs), new object[] { new string[] { "CatalogS2L2A" } })]
        public async Task PostItemSearchAsync(StacApiApplication application)
        {
            var client = application.CreateClient();
            ItemSearchClient itemSearchClient = new ItemSearchClient(client);

            var polygon = new Polygon(new double[][][]
                {
                    new double[][]
                    {
                        new double[] { 115.6433814, 57.6452553 },
                        new double[] { 115.6540305, 57.6649078 },
                        new double[] { 115.7314869, 57.809336 },
                        new double[] { 115.8109064, 57.9534193 },
                        new double[] { 115.8872017, 58.0983519 },
                        new double[] { 115.967219, 58.2424627 },
                        new double[] { 116.0459009, 58.3869795 },
                        new double[] { 116.1261709, 58.5312986 },
                        new double[] { 116.1846498, 58.6346043 },
                        new double[] { 117.1681312, 58.6405464 },
                        new double[] { 117.1635452, 57.6543437 },
                        new double[] { 115.6433814, 57.6452553 }
                    }
                });

            SearchBody body = new SearchBody()
            {
                Intersects = new Stac.Api.Models.Core.IntersectGeometryFilter(polygon),
                Limit = 10,
                Collections = new string[] { "sentinel-2-l2a" }
            };

            var result = await itemSearchClient.PostItemSearchAsync(body);

            Assert.NotNull(result);
            Assert.NotNull(result.Features);
            Assert.NotEmpty(result.Features);
            Assert.NotNull(result.Links);
            Assert.NotEmpty(result.Links);
            Assert.Equal(6, result.Features.Count);
            foreach (var feature in result.Features)
            {
                Assert.True(feature.Geometry.Intersects(polygon));
            }

        }
    }
}
