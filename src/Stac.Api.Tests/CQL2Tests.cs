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
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace Stac.Api.Tests
{
    public class CQL2Tests : TestBase
    {
        private readonly JsonSerializerSettings _settings;

        public CQL2Tests(ITestOutputHelper outputHelper) : base(outputHelper)
        {
            _settings = new JsonSerializerSettings();
            _settings.Converters.Add(new BooleanExpressionConverter());
        }

        [Fact]
        public async Task Example1Test()
        {
            var json = GetJson("CQL2", "Example1");
            JObject jObject = JObject.Parse(json);
            var cql = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            Assert.IsType<AndOrExpression>(cql);
            Assert.NotNull(cql.AsAndOrExpression());
            Assert.Equal(AndOrExpressionOp.And, cql.AsAndOrExpression().Op);
            Assert.Equal(2, cql.AsAndOrExpression().Args.Count);
            Assert.IsType<BinaryComparisonPredicate>(cql.AsAndOrExpression().Args[0]);
            Assert.NotNull(cql.AsAndOrExpression().Args[0].AsComparison());
            Assert.Equal(ComparisonPredicateOp.Eq, cql.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Op);
            Assert.Equal(2, cql.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Args.Count);
            Assert.IsType<PropertyRef>(cql.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Args[0]);
            Assert.Equal("id", cql.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Args[0].AsCharExpression().AsPropertyRef().Property);
            Assert.Equal("S2A_60HWB_20201111_0_L2A", cql.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Args[1].AsCharExpression().ToString());
        }

        [Fact]
        public async Task Example2Test()
        {
            var json = GetJson("CQL2", "Example2");
            JObject jObject = JObject.Parse(json);
            var cql = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            Assert.IsType<AndOrExpression>(cql);
            Assert.NotNull(cql.AsAndOrExpression());
            Assert.Equal(AndOrExpressionOp.And, cql.AsAndOrExpression().Op);
            Assert.Equal(4, cql.AsAndOrExpression().Args.Count);
            Assert.IsType<BinaryComparisonPredicate>(cql.AsAndOrExpression().Args[0]);
            Assert.NotNull(cql.AsAndOrExpression().Args[0].AsComparison());

            Assert.Equal(ComparisonPredicateOp.Eq, cql.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Op);
            Assert.Equal(2, cql.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Args.Count);
            Assert.IsType<PropertyRef>(cql.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Args[0]);
            Assert.Equal("collection", cql.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Args[0].AsCharExpression().AsPropertyRef().Property);
            Assert.Equal("landsat8_l1tp", cql.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Args[1].AsCharExpression().ToString());

            Assert.Equal(ComparisonPredicateOp.Le, cql.AsAndOrExpression().Args[1].AsComparison().AsBinaryComparison().Op);
            Assert.Equal(2, cql.AsAndOrExpression().Args[1].AsComparison().AsBinaryComparison().Args.Count);
            Assert.IsType<PropertyRef>(cql.AsAndOrExpression().Args[1].AsComparison().AsBinaryComparison().Args[0]);
            Assert.Equal("eo:cloud_cover", cql.AsAndOrExpression().Args[1].AsComparison().AsBinaryComparison().Args[0].AsCharExpression().AsPropertyRef().Property);
            Assert.Equal(10, cql.AsAndOrExpression().Args[1].AsComparison().AsBinaryComparison().Args[1].AsNumericExpression().AsNumber().Num);

            Assert.Equal(ComparisonPredicateOp.Ge, cql.AsAndOrExpression().Args[2].AsComparison().AsBinaryComparison().Op);
            Assert.Equal(2, cql.AsAndOrExpression().Args[2].AsComparison().AsBinaryComparison().Args.Count);
            Assert.IsType<PropertyRef>(cql.AsAndOrExpression().Args[2].AsComparison().AsBinaryComparison().Args[0]);
            Assert.Equal("datetime", cql.AsAndOrExpression().Args[2].AsComparison().AsBinaryComparison().Args[0].AsCharExpression().AsPropertyRef().Property);
            DateTimeOffset datetime = DateTimeOffset.Parse("2021-04-08T04:39:23Z");
            Assert.Equal(datetime, cql.AsAndOrExpression().Args[2].AsComparison().AsBinaryComparison().Args[1].AsTemporalInstant().DateTime);

            Assert.Equal(SpatialPredicateOp.S_intersects, cql.AsAndOrExpression().Args[3].AsComparison().AsSpatialPredicate().Op);
            Assert.Equal(2, cql.AsAndOrExpression().Args[3].AsComparison().AsSpatialPredicate().Args.Count);
            Assert.IsType<PropertyRef>(cql.AsAndOrExpression().Args[3].AsComparison().AsSpatialPredicate().Args[0]);
            Assert.Equal("geometry", cql.AsAndOrExpression().Args[3].AsComparison().AsSpatialPredicate().Args[0].AsPropertyRef().Property);
            Assert.IsType<Polygon>(cql.AsAndOrExpression().Args[3].AsComparison().AsSpatialPredicate().Args[1].AsSpatialLiteral().AsGeometryLiteral().GeometryObject);
        }

        [Fact]
        public async Task Example3Test()
        {
            var json = GetJson("CQL2", "Example3");
            JObject jObject = JObject.Parse(json);
            var cql = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            Assert.IsType<AndOrExpression>(cql);
            Assert.NotNull(cql.AsAndOrExpression());
            Assert.Equal(AndOrExpressionOp.And, cql.AsAndOrExpression().Op);
            Assert.Equal(2, cql.AsAndOrExpression().Args.Count);
            Assert.IsType<BinaryComparisonPredicate>(cql.AsAndOrExpression().Args[0]);
            Assert.NotNull(cql.AsAndOrExpression().Args[0].AsComparison());

            Assert.Equal(ComparisonPredicateOp.Gt, cql.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Op);
            Assert.Equal(2, cql.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Args.Count);
            Assert.IsType<PropertyRef>(cql.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Args[0]);
            Assert.Equal("sentinel:data_coverage", cql.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Args[0].AsCharExpression().AsPropertyRef().Property);
            Assert.Equal(50, cql.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Args[1].AsNumericExpression().AsNumber().Num);

            Assert.Equal(ComparisonPredicateOp.Lt, cql.AsAndOrExpression().Args[1].AsComparison().AsBinaryComparison().Op);
            Assert.Equal(2, cql.AsAndOrExpression().Args[1].AsComparison().AsBinaryComparison().Args.Count);
            Assert.IsType<PropertyRef>(cql.AsAndOrExpression().Args[1].AsComparison().AsBinaryComparison().Args[0]);
            Assert.Equal("eo:cloud_cover", cql.AsAndOrExpression().Args[1].AsComparison().AsBinaryComparison().Args[0].AsCharExpression().AsPropertyRef().Property);
            Assert.Equal(10, cql.AsAndOrExpression().Args[1].AsComparison().AsBinaryComparison().Args[1].AsNumericExpression().AsNumber().Num);

        }

        [Fact]
        public async Task Example4Test()
        {
            var json = GetJson("CQL2", "Example4");
            JObject jObject = JObject.Parse(json);
            var cql = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            Assert.IsType<AndOrExpression>(cql);
            Assert.NotNull(cql.AsAndOrExpression());
            Assert.Equal(AndOrExpressionOp.Or, cql.AsAndOrExpression().Op);
            Assert.Equal(2, cql.AsAndOrExpression().Args.Count);
            Assert.IsType<BinaryComparisonPredicate>(cql.AsAndOrExpression().Args[0]);
            Assert.NotNull(cql.AsAndOrExpression().Args[0].AsComparison());

            Assert.Equal(ComparisonPredicateOp.Gt, cql.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Op);
            Assert.Equal(2, cql.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Args.Count);
            Assert.IsType<PropertyRef>(cql.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Args[0]);
            Assert.Equal("sentinel:data_coverage", cql.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Args[0].AsCharExpression().AsPropertyRef().Property);
            Assert.Equal(50, cql.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Args[1].AsNumericExpression().AsNumber().Num);

            Assert.Equal(ComparisonPredicateOp.Lt, cql.AsAndOrExpression().Args[1].AsComparison().AsBinaryComparison().Op);
            Assert.Equal(2, cql.AsAndOrExpression().Args[1].AsComparison().AsBinaryComparison().Args.Count);
            Assert.IsType<PropertyRef>(cql.AsAndOrExpression().Args[1].AsComparison().AsBinaryComparison().Args[0]);
            Assert.Equal("eo:cloud_cover", cql.AsAndOrExpression().Args[1].AsComparison().AsBinaryComparison().Args[0].AsCharExpression().AsPropertyRef().Property);
            Assert.Equal(10, cql.AsAndOrExpression().Args[1].AsComparison().AsBinaryComparison().Args[1].AsNumericExpression().AsNumber().Num);

        }

        [Fact]
        public async Task Example5Test()
        {
            var json = GetJson("CQL2", "Example5");
            JObject jObject = JObject.Parse(json);
            var cql = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            Assert.IsType<BinaryComparisonPredicate>(cql);
            Assert.NotNull(cql.AsComparison().AsBinaryComparison());
            Assert.Equal(ComparisonPredicateOp.Eq, cql.AsComparison().AsBinaryComparison().Op);
            Assert.Equal(2, cql.AsComparison().AsBinaryComparison().Args.Count);

            Assert.Equal("prop1", cql.AsComparison().AsBinaryComparison().Args[0].AsCharExpression().AsPropertyRef().Property);
            Assert.Equal("prop2", cql.AsComparison().AsBinaryComparison().Args[1].AsCharExpression().AsPropertyRef().Property);

        }

        [Fact]
        public async Task Example6Test()
        {
            var json = GetJson("CQL2", "Example6");
            JObject jObject = JObject.Parse(json);
            var cql = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            Assert.IsType<TemporalPredicate>(cql);
            Assert.NotNull(cql.AsComparison().AsTemporalPredicate());
            Assert.Equal(TemporalPredicateOp.T_intersects, cql.AsComparison().AsTemporalPredicate().Op);
            Assert.Equal(2, cql.AsComparison().AsTemporalPredicate().Args.Count);

            Assert.Equal("datetime", cql.AsComparison().AsTemporalPredicate().Args[0].AsPropertyRef().Property);
            var date1 = DateTimeOffset.Parse("2020-11-11T00:00:00Z", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            var date2 = DateTimeOffset.Parse("2020-11-12T00:00:00Z", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            Itenso.TimePeriod.TimeInterval interval = new Itenso.TimePeriod.TimeInterval(date1.Date, date2.Date);
            Assert.Equal(interval, cql.AsComparison().AsTemporalPredicate().Args[1].AsTemporalLiteral().AsIntervalLiteral().TimeInterval);

        }

        [Fact]
        public async Task Example7Test()
        {
            var json = GetJson("CQL2", "Example7");
            JObject jObject = JObject.Parse(json);
            var cql = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            Assert.IsType<SpatialPredicate>(cql);
            Assert.NotNull(cql.AsComparison().AsSpatialPredicate());
            Assert.Equal(SpatialPredicateOp.S_intersects, cql.AsComparison().AsSpatialPredicate().Op);
            Assert.Equal(2, cql.AsComparison().AsSpatialPredicate().Args.Count);

            Assert.Equal("geometry", cql.AsComparison().AsSpatialPredicate().Args[0].AsPropertyRef().Property);
            Assert.IsType<Polygon>(cql.AsComparison().AsSpatialPredicate().Args[1].AsSpatialLiteral().AsGeometryLiteral().GeometryObject);

        }

        [Fact]
        public async Task Example8Test()
        {
            var json = GetJson("CQL2", "Example8");
            JObject jObject = JObject.Parse(json);
            var cql = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            Assert.IsType<AndOrExpression>(cql);
            Assert.NotNull(cql.AsAndOrExpression());
            Assert.Equal(AndOrExpressionOp.Or, cql.AsAndOrExpression().Op);
            Assert.Equal(2, cql.AsAndOrExpression().Args.Count);

            Assert.Equal(SpatialPredicateOp.S_intersects, cql.AsAndOrExpression().Args[0].AsComparison().AsSpatialPredicate().Op);
            Assert.Equal("geometry", cql.AsAndOrExpression().Args[0].AsComparison().AsSpatialPredicate().Args[0].AsPropertyRef().Property);
            Assert.IsType<Polygon>(cql.AsAndOrExpression().Args[0].AsComparison().AsSpatialPredicate().Args[1].AsSpatialLiteral().AsGeometryLiteral().GeometryObject);

            Assert.Equal(SpatialPredicateOp.S_intersects, cql.AsAndOrExpression().Args[1].AsComparison().AsSpatialPredicate().Op);
            Assert.Equal("geometry", cql.AsAndOrExpression().Args[1].AsComparison().AsSpatialPredicate().Args[0].AsPropertyRef().Property);
            Assert.IsType<Polygon>(cql.AsAndOrExpression().Args[1].AsComparison().AsSpatialPredicate().Args[1].AsSpatialLiteral().AsGeometryLiteral().GeometryObject);

        }

        [Fact]
        public async Task Example9Test()
        {
            var json = GetJson("CQL2", "Example9");
            JObject jObject = JObject.Parse(json);
            var cql = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            Assert.IsType<AndOrExpression>(cql);
            Assert.NotNull(cql.AsAndOrExpression());
            Assert.Equal(AndOrExpressionOp.Or, cql.AsAndOrExpression().Op);
            Assert.Equal(3, cql.AsAndOrExpression().Args.Count);

            Assert.Equal(ComparisonPredicateOp.Gt, cql.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Op);
            Assert.Equal("sentinel:data_coverage", cql.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Args[0].AsCharExpression().AsPropertyRef().Property);
            Assert.Equal(50, cql.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Args[1].AsNumericExpression().AsNumber().Num);

            Assert.Equal(ComparisonPredicateOp.Lt, cql.AsAndOrExpression().Args[1].AsComparison().AsBinaryComparison().Op);
            Assert.Equal("landsat:coverage_percent", cql.AsAndOrExpression().Args[1].AsComparison().AsBinaryComparison().Args[0].AsCharExpression().AsPropertyRef().Property);
            Assert.Equal(10, cql.AsAndOrExpression().Args[1].AsComparison().AsBinaryComparison().Args[1].AsNumericExpression().AsNumber().Num);

            Assert.Equal(AndOrExpressionOp.And, cql.AsAndOrExpression().Args[2].AsAndOrExpression().Op);
            Assert.IsType<IsNullPredicate>(cql.AsAndOrExpression().Args[2].AsAndOrExpression().Args[0]);
            Assert.IsType<IsNullPredicate>(cql.AsAndOrExpression().Args[2].AsAndOrExpression().Args[1]);
            Assert.Equal("sentinel:data_coverage", cql.AsAndOrExpression().Args[2].AsAndOrExpression().Args[0].AsComparison().AsIsNullPredicate().Args.AsCharExpression().AsPropertyRef().Property);
            Assert.Equal("landsat:coverage_percent", cql.AsAndOrExpression().Args[2].AsAndOrExpression().Args[1].AsComparison().AsIsNullPredicate().Args.AsCharExpression().AsPropertyRef().Property);

        }

        [Fact]
        public async Task Example10Test()
        {
            var json = GetJson("CQL2", "Example10");
            JObject jObject = JObject.Parse(json);
            var cql = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            Assert.IsType<IsBetweenPredicate>(cql);
            Assert.NotNull(cql.AsComparison().AsIsBetweenPredicate());
            Assert.Equal(IsBetweenPredicateOp.Between, cql.AsComparison().AsIsBetweenPredicate().Op);
            Assert.Equal(3, cql.AsComparison().AsIsBetweenPredicate().Args.Count);

            Assert.Equal("eo:cloud_cover", cql.AsComparison().AsIsBetweenPredicate().Args[0].AsCharExpression().AsPropertyRef().Property);
            Assert.Equal(0.0, cql.AsComparison().AsIsBetweenPredicate().Args[1].AsNumeric().Value);
            Assert.Equal(50.0, cql.AsComparison().AsIsBetweenPredicate().Args[2].AsNumeric().Value);

        }

        [Fact]
        public async Task Example11Test()
        {
            var json = GetJson("CQL2", "Example11");
            JObject jObject = JObject.Parse(json);
            var cql = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            Assert.IsType<IsLikePredicate>(cql);
            Assert.NotNull(cql.AsComparison().AsIsLikePredicate());
            Assert.Equal(IsLikePredicateOp.Like, cql.AsComparison().AsIsLikePredicate().Op);
            Assert.Equal(2, cql.AsComparison().AsIsLikePredicate().Args.Count);

            Assert.Equal("mission", cql.AsComparison().AsIsLikePredicate().Args[0].AsPropertyRef().Property);
            Assert.IsType<Models.Cql2.String>(cql.AsComparison().AsIsLikePredicate().Args[1]);
            Assert.Equal("sentinel%", cql.AsComparison().AsIsLikePredicate().Args[1].ToString());

        }

        [Fact]
        public async Task Example12Test()
        {
            var json = GetJson("CQL2", "Example12");
            JObject jObject = JObject.Parse(json);
            var cql = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            Assert.IsType<IsInListPredicate>(cql);
            Assert.NotNull(cql.AsComparison().AsIsInListPredicate());
            Assert.Equal(IsInListPredicateOp.In, cql.AsComparison().AsIsInListPredicate().Op);
            Assert.Equal(2, cql.AsComparison().AsIsInListPredicate().Args.Count);

            Assert.Equal("keywords", cql.AsComparison().AsIsInListPredicate().Args[0].AsScalarExpression().AsCharExpression().AsPropertyRef().Property);
            Assert.IsType<ScalarExpressionCollection>(cql.AsComparison().AsIsInListPredicate().Args[1]);
            Assert.Equal<string>(new List<string>(){ "fire", "forest", "wildfire" }, cql.AsComparison().AsIsInListPredicate().Args[1].AsScalarExpressionCollection().Select(x => x.AsCharExpression().ToString()).ToList());
        }
    }
}
