using Xunit;
using System.Threading.Tasks;
using Xunit.Abstractions;
using System.Linq;
using GeoJSON.Net.Geometry;
using Stac.Api.Extensions.Filters;
using Stac.Api.Clients.Extensions.Filter;
using Stac.Api.Models.Cql2;
using Newtonsoft.Json;
using Stac.Api.Converters;
using Stac.Api.Models;

namespace Stac.Api.Tests.AppTests
{
    [Collection(StacApiAppCollectionFixture.Name)]
    public class FilterApiTests : AppTestBase
    {
        private readonly JsonSerializerSettings _settings;

        public FilterApiTests(ITestOutputHelper outputHelper,
                              TestCatalogsProvider testCatalogsProvider) : base(outputHelper)
        {
            _settings = new JsonSerializerSettings();
            _settings.Converters.Add(new BooleanExpressionConverter());
            TestCatalogsProvider = testCatalogsProvider;
            TestCatalogsProvider.SetOutputHelper(outputHelper);
        }

        public TestCatalogsProvider TestCatalogsProvider { get; }

        [Theory, InlineData( "CatalogS2L2A" )]
        public async Task GetItemSearchAsync(string catalogName)
        {
            var application = TestCatalogsProvider.GetStacApiApplication(catalogName);
            var client = application.CreateClient();
            FilterClient filterClient = new FilterClient(client);
            filterClient.ReadResponseAsString = true;

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

            SpatialPredicate spatialFilter = new SpatialPredicate();
            spatialFilter.Op = SpatialPredicateOp.S_intersects;
            spatialFilter.Args.Add(new PropertyRef("geometry"));
            spatialFilter.Args.Add(new GeometryLiteral(polygon));
            CQL2Expression filter = new CQL2Expression(spatialFilter);

            StacFeatureCollection result = await filterClient.GetItemSearchAsync(
                filter,
                Converters.CQL2FilterConverter.FilterLang.Cql2Json,
                null
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


    }
}
