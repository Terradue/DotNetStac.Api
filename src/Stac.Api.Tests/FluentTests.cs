using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoJSON.Net.Geometry;
using Stac.Api.Clients;
using Stac.Api.Clients.Fluent;
using Stac.Api.Extensions.Filters;
using Xunit;
using Xunit.Abstractions;

namespace Stac.Api.Tests
{
    [Collection(StacApiAppCollectionFixture.Name)]
    public class FluentTests : AppTestBase
    {
        public FluentTests(ITestOutputHelper outputHelper,
                           TestCatalogsProvider testCatalogsProvider) : base(outputHelper)
        {
            TestCatalogsProvider = testCatalogsProvider;
            TestCatalogsProvider.SetOutputHelper(outputHelper);
        }

        public TestCatalogsProvider TestCatalogsProvider { get; }

        [Theory, InlineData( "CatalogS2L2A" )]
        public async Task SearchFeatures(string catalogName)
        {
            StacApiApplication application = TestCatalogsProvider.GetStacApiApplication(catalogName);
            var client = application.CreateClient();
            ApiClient apiClient = new ApiClient(client);

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

            var result = await apiClient.ItemSearch.Search()
                .From("sentinel-2-l2a")
                .Intersects(polygon)
                .Limit(10)
                .ExecuteAsync();

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