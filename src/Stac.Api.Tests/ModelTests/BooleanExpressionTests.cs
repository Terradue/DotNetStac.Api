using System;
using Xunit;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Api.Models.Cql2;
using Stac.Api.Converters;
using GeoJSON.Net.Geometry;
using System.Globalization;

namespace Stac.Api.Tests
{
    public class BooleanExpressionTests : TestBase
    {
        private readonly JsonSerializerSettings _settings;

        public BooleanExpressionTests(ITestOutputHelper outputHelper) : base(outputHelper)
        {
            _settings = new JsonSerializerSettings();
            _settings.Converters.Add(new BooleanExpressionConverter());
        }

        [Fact]
        public async Task Example1Test()
        {
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

            SpatialPredicate filter = new SpatialPredicate();
            filter.Op = SpatialPredicateOp.S_intersects;
            filter.Args.Add(new PropertyRef("geometry"));
            filter.Args.Add(new GeometryLiteral(polygon));
            CQL2Expression cql2Filter = new CQL2Expression(filter);

            var cql2json = System.Convert.ToString(cql2Filter);
            var result = JsonConvert.DeserializeObject<BooleanExpression>(cql2json, _settings);

            JsonAssert.AreEqual(cql2json, filter.ToString());
        }
    }
}
