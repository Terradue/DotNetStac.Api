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
            var be = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            Assert.IsType<AndOrExpression>(be);
            Assert.NotNull(be.AsAndOrExpression());
            Assert.Equal(AndOrExpressionOp.And, be.AsAndOrExpression().Op);
            Assert.Equal(2, be.AsAndOrExpression().Args.Count);
            Assert.IsType<BinaryComparisonPredicate>(be.AsAndOrExpression().Args[0]);
            Assert.NotNull(be.AsAndOrExpression().Args[0].AsComparison());
            Assert.Equal(ComparisonPredicateOp.Eq, be.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Op);
            Assert.Equal(2, be.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Args.Count);
            Assert.IsType<PropertyRef>(be.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Args[0]);
            Assert.Equal("id", be.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Args[0].AsCharExpression().AsPropertyRef().Property);
            Assert.Equal("S2A_60HWB_20201111_0_L2A", be.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Args[1].AsCharExpression().ToString());
            var json2 = JsonConvert.SerializeObject(new CQL2Expression(be), _settings);
            JsonAssert.AreEqual(json, json2);
        }

        [Fact]
        public async Task Example2Test()
        {
            var json = GetJson("CQL2", "Example2");
            JObject jObject = JObject.Parse(json);
            var be = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            Assert.IsType<AndOrExpression>(be);
            Assert.NotNull(be.AsAndOrExpression());
            Assert.Equal(AndOrExpressionOp.And, be.AsAndOrExpression().Op);
            Assert.Equal(4, be.AsAndOrExpression().Args.Count);
            Assert.IsType<BinaryComparisonPredicate>(be.AsAndOrExpression().Args[0]);
            Assert.NotNull(be.AsAndOrExpression().Args[0].AsComparison());

            Assert.Equal(ComparisonPredicateOp.Eq, be.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Op);
            Assert.Equal(2, be.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Args.Count);
            Assert.IsType<PropertyRef>(be.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Args[0]);
            Assert.Equal("collection", be.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Args[0].AsCharExpression().AsPropertyRef().Property);
            Assert.Equal("landsat8_l1tp", be.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Args[1].AsCharExpression().ToString());

            Assert.Equal(ComparisonPredicateOp.Le, be.AsAndOrExpression().Args[1].AsComparison().AsBinaryComparison().Op);
            Assert.Equal(2, be.AsAndOrExpression().Args[1].AsComparison().AsBinaryComparison().Args.Count);
            Assert.IsType<PropertyRef>(be.AsAndOrExpression().Args[1].AsComparison().AsBinaryComparison().Args[0]);
            Assert.Equal("eo:cloud_cover", be.AsAndOrExpression().Args[1].AsComparison().AsBinaryComparison().Args[0].AsCharExpression().AsPropertyRef().Property);
            Assert.Equal(10, be.AsAndOrExpression().Args[1].AsComparison().AsBinaryComparison().Args[1].AsNumericExpression().AsNumber().Num);

            Assert.Equal(ComparisonPredicateOp.Ge, be.AsAndOrExpression().Args[2].AsComparison().AsBinaryComparison().Op);
            Assert.Equal(2, be.AsAndOrExpression().Args[2].AsComparison().AsBinaryComparison().Args.Count);
            Assert.IsType<PropertyRef>(be.AsAndOrExpression().Args[2].AsComparison().AsBinaryComparison().Args[0]);
            Assert.Equal("datetime", be.AsAndOrExpression().Args[2].AsComparison().AsBinaryComparison().Args[0].AsCharExpression().AsPropertyRef().Property);
            DateTimeOffset datetime = DateTimeOffset.Parse("2021-04-08T04:39:23Z");
            Assert.Equal(datetime, be.AsAndOrExpression().Args[2].AsComparison().AsBinaryComparison().Args[1].AsTemporalInstant().DateTime);

            Assert.Equal(SpatialPredicateOp.S_intersects, be.AsAndOrExpression().Args[3].AsComparison().AsSpatialPredicate().Op);
            Assert.Equal(2, be.AsAndOrExpression().Args[3].AsComparison().AsSpatialPredicate().Args.Count);
            Assert.IsType<PropertyRef>(be.AsAndOrExpression().Args[3].AsComparison().AsSpatialPredicate().Args[0]);
            Assert.Equal("geometry", be.AsAndOrExpression().Args[3].AsComparison().AsSpatialPredicate().Args[0].AsPropertyRef().Property);
            Assert.IsType<Polygon>(be.AsAndOrExpression().Args[3].AsComparison().AsSpatialPredicate().Args[1].AsSpatialLiteral().AsGeometryLiteral().GeometryObject);

            var json2 = JsonConvert.SerializeObject(new CQL2Expression(be), _settings);
            JsonAssert.AreEqual(json, json2);
        }

        [Fact]
        public async Task Example3Test()
        {
            var json = GetJson("CQL2", "Example3");
            JObject jObject = JObject.Parse(json);
            var be = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            Assert.IsType<AndOrExpression>(be);
            Assert.NotNull(be.AsAndOrExpression());
            Assert.Equal(AndOrExpressionOp.And, be.AsAndOrExpression().Op);
            Assert.Equal(2, be.AsAndOrExpression().Args.Count);
            Assert.IsType<BinaryComparisonPredicate>(be.AsAndOrExpression().Args[0]);
            Assert.NotNull(be.AsAndOrExpression().Args[0].AsComparison());

            Assert.Equal(ComparisonPredicateOp.Gt, be.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Op);
            Assert.Equal(2, be.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Args.Count);
            Assert.IsType<PropertyRef>(be.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Args[0]);
            Assert.Equal("sentinel:data_coverage", be.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Args[0].AsCharExpression().AsPropertyRef().Property);
            Assert.Equal(50, be.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Args[1].AsNumericExpression().AsNumber().Num);

            Assert.Equal(ComparisonPredicateOp.Lt, be.AsAndOrExpression().Args[1].AsComparison().AsBinaryComparison().Op);
            Assert.Equal(2, be.AsAndOrExpression().Args[1].AsComparison().AsBinaryComparison().Args.Count);
            Assert.IsType<PropertyRef>(be.AsAndOrExpression().Args[1].AsComparison().AsBinaryComparison().Args[0]);
            Assert.Equal("eo:cloud_cover", be.AsAndOrExpression().Args[1].AsComparison().AsBinaryComparison().Args[0].AsCharExpression().AsPropertyRef().Property);
            Assert.Equal(10, be.AsAndOrExpression().Args[1].AsComparison().AsBinaryComparison().Args[1].AsNumericExpression().AsNumber().Num);

            var json2 = JsonConvert.SerializeObject(new CQL2Expression(be), _settings);
            JsonAssert.AreEqual(json, json2);
        }

        [Fact]
        public async Task Example4Test()
        {
            var json = GetJson("CQL2", "Example4");
            JObject jObject = JObject.Parse(json);
            var be = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            Assert.IsType<AndOrExpression>(be);
            Assert.NotNull(be.AsAndOrExpression());
            Assert.Equal(AndOrExpressionOp.Or, be.AsAndOrExpression().Op);
            Assert.Equal(2, be.AsAndOrExpression().Args.Count);
            Assert.IsType<BinaryComparisonPredicate>(be.AsAndOrExpression().Args[0]);
            Assert.NotNull(be.AsAndOrExpression().Args[0].AsComparison());

            Assert.Equal(ComparisonPredicateOp.Gt, be.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Op);
            Assert.Equal(2, be.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Args.Count);
            Assert.IsType<PropertyRef>(be.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Args[0]);
            Assert.Equal("sentinel:data_coverage", be.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Args[0].AsCharExpression().AsPropertyRef().Property);
            Assert.Equal(50, be.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Args[1].AsNumericExpression().AsNumber().Num);

            Assert.Equal(ComparisonPredicateOp.Lt, be.AsAndOrExpression().Args[1].AsComparison().AsBinaryComparison().Op);
            Assert.Equal(2, be.AsAndOrExpression().Args[1].AsComparison().AsBinaryComparison().Args.Count);
            Assert.IsType<PropertyRef>(be.AsAndOrExpression().Args[1].AsComparison().AsBinaryComparison().Args[0]);
            Assert.Equal("eo:cloud_cover", be.AsAndOrExpression().Args[1].AsComparison().AsBinaryComparison().Args[0].AsCharExpression().AsPropertyRef().Property);
            Assert.Equal(10, be.AsAndOrExpression().Args[1].AsComparison().AsBinaryComparison().Args[1].AsNumericExpression().AsNumber().Num);

            var json2 = JsonConvert.SerializeObject(new CQL2Expression(be), _settings);
            JsonAssert.AreEqual(json, json2);
        }

        [Fact]
        public async Task Example5Test()
        {
            var json = GetJson("CQL2", "Example5");
            JObject jObject = JObject.Parse(json);
            var be = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            Assert.IsType<BinaryComparisonPredicate>(be);
            Assert.NotNull(be.AsComparison().AsBinaryComparison());
            Assert.Equal(ComparisonPredicateOp.Eq, be.AsComparison().AsBinaryComparison().Op);
            Assert.Equal(2, be.AsComparison().AsBinaryComparison().Args.Count);

            Assert.Equal("prop1", be.AsComparison().AsBinaryComparison().Args[0].AsCharExpression().AsPropertyRef().Property);
            Assert.Equal("prop2", be.AsComparison().AsBinaryComparison().Args[1].AsCharExpression().AsPropertyRef().Property);

            var json2 = JsonConvert.SerializeObject(new CQL2Expression(be), _settings);
            JsonAssert.AreEqual(json, json2);
        }

        [Fact]
        public async Task Example6Test()
        {
            var json = GetJson("CQL2", "Example6");
            JObject jObject = JObject.Parse(json);
            var be = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            Assert.IsType<TemporalPredicate>(be);
            Assert.NotNull(be.AsComparison().AsTemporalPredicate());
            Assert.Equal(TemporalPredicateOp.T_intersects, be.AsComparison().AsTemporalPredicate().Op);
            Assert.Equal(2, be.AsComparison().AsTemporalPredicate().Args.Count);

            Assert.Equal("datetime", be.AsComparison().AsTemporalPredicate().Args[0].AsPropertyRef().Property);
            var date1 = DateTimeOffset.Parse("2020-11-11T00:00:00Z", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            var date2 = DateTimeOffset.Parse("2020-11-12T00:00:00Z", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            Itenso.TimePeriod.TimeInterval interval = new Itenso.TimePeriod.TimeInterval(date1.Date, date2.Date);
            Assert.Equal(interval, be.AsComparison().AsTemporalPredicate().Args[1].AsTemporalLiteral().AsIntervalLiteral().TimeInterval);

            var json2 = JsonConvert.SerializeObject(new CQL2Expression(be), _settings);
            JsonAssert.AreEqual(json, json2);
        }

        [Fact]
        public async Task Example7Test()
        {
            var json = GetJson("CQL2", "Example7");
            JObject jObject = JObject.Parse(json);
            var be = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            Assert.IsType<SpatialPredicate>(be);
            Assert.NotNull(be.AsComparison().AsSpatialPredicate());
            Assert.Equal(SpatialPredicateOp.S_intersects, be.AsComparison().AsSpatialPredicate().Op);
            Assert.Equal(2, be.AsComparison().AsSpatialPredicate().Args.Count);

            Assert.Equal("geometry", be.AsComparison().AsSpatialPredicate().Args[0].AsPropertyRef().Property);
            Assert.IsType<Polygon>(be.AsComparison().AsSpatialPredicate().Args[1].AsSpatialLiteral().AsGeometryLiteral().GeometryObject);

            var json2 = JsonConvert.SerializeObject(new CQL2Expression(be), _settings);
            JsonAssert.AreEqual(json, json2);
        }

        [Fact]
        public async Task Example8Test()
        {
            var json = GetJson("CQL2", "Example8");
            JObject jObject = JObject.Parse(json);
            var be = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            Assert.IsType<AndOrExpression>(be);
            Assert.NotNull(be.AsAndOrExpression());
            Assert.Equal(AndOrExpressionOp.Or, be.AsAndOrExpression().Op);
            Assert.Equal(2, be.AsAndOrExpression().Args.Count);

            Assert.Equal(SpatialPredicateOp.S_intersects, be.AsAndOrExpression().Args[0].AsComparison().AsSpatialPredicate().Op);
            Assert.Equal("geometry", be.AsAndOrExpression().Args[0].AsComparison().AsSpatialPredicate().Args[0].AsPropertyRef().Property);
            Assert.IsType<Polygon>(be.AsAndOrExpression().Args[0].AsComparison().AsSpatialPredicate().Args[1].AsSpatialLiteral().AsGeometryLiteral().GeometryObject);

            Assert.Equal(SpatialPredicateOp.S_intersects, be.AsAndOrExpression().Args[1].AsComparison().AsSpatialPredicate().Op);
            Assert.Equal("geometry", be.AsAndOrExpression().Args[1].AsComparison().AsSpatialPredicate().Args[0].AsPropertyRef().Property);
            Assert.IsType<Polygon>(be.AsAndOrExpression().Args[1].AsComparison().AsSpatialPredicate().Args[1].AsSpatialLiteral().AsGeometryLiteral().GeometryObject);

            var json2 = JsonConvert.SerializeObject(new CQL2Expression(be), _settings);
            JsonAssert.AreEqual(json, json2);
        }

        [Fact]
        public async Task Example9Test()
        {
            var json = GetJson("CQL2", "Example9");
            JObject jObject = JObject.Parse(json);
            var be = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            Assert.IsType<AndOrExpression>(be);
            Assert.NotNull(be.AsAndOrExpression());
            Assert.Equal(AndOrExpressionOp.Or, be.AsAndOrExpression().Op);
            Assert.Equal(3, be.AsAndOrExpression().Args.Count);

            Assert.Equal(ComparisonPredicateOp.Gt, be.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Op);
            Assert.Equal("sentinel:data_coverage", be.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Args[0].AsCharExpression().AsPropertyRef().Property);
            Assert.Equal(50, be.AsAndOrExpression().Args[0].AsComparison().AsBinaryComparison().Args[1].AsNumericExpression().AsNumber().Num);

            Assert.Equal(ComparisonPredicateOp.Lt, be.AsAndOrExpression().Args[1].AsComparison().AsBinaryComparison().Op);
            Assert.Equal("landsat:coverage_percent", be.AsAndOrExpression().Args[1].AsComparison().AsBinaryComparison().Args[0].AsCharExpression().AsPropertyRef().Property);
            Assert.Equal(10, be.AsAndOrExpression().Args[1].AsComparison().AsBinaryComparison().Args[1].AsNumericExpression().AsNumber().Num);

            Assert.Equal(AndOrExpressionOp.And, be.AsAndOrExpression().Args[2].AsAndOrExpression().Op);
            Assert.IsType<IsNullPredicate>(be.AsAndOrExpression().Args[2].AsAndOrExpression().Args[0]);
            Assert.IsType<IsNullPredicate>(be.AsAndOrExpression().Args[2].AsAndOrExpression().Args[1]);
            Assert.Equal("sentinel:data_coverage", be.AsAndOrExpression().Args[2].AsAndOrExpression().Args[0].AsComparison().AsIsNullPredicate().Args.AsCharExpression().AsPropertyRef().Property);
            Assert.Equal("landsat:coverage_percent", be.AsAndOrExpression().Args[2].AsAndOrExpression().Args[1].AsComparison().AsIsNullPredicate().Args.AsCharExpression().AsPropertyRef().Property);

            var json2 = JsonConvert.SerializeObject(new CQL2Expression(be), _settings);
            JsonAssert.AreEqual(json, json2);
        }

        [Fact]
        public async Task Example10Test()
        {
            var json = GetJson("CQL2", "Example10");
            JObject jObject = JObject.Parse(json);
            var be = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            Assert.IsType<IsBetweenPredicate>(be);
            Assert.NotNull(be.AsComparison().AsIsBetweenPredicate());
            Assert.Equal(IsBetweenPredicateOp.Between, be.AsComparison().AsIsBetweenPredicate().Op);
            Assert.Equal(3, be.AsComparison().AsIsBetweenPredicate().Args.Count);

            Assert.Equal("eo:cloud_cover", be.AsComparison().AsIsBetweenPredicate().Args[0].AsCharExpression().AsPropertyRef().Property);
            Assert.Equal(0.0, be.AsComparison().AsIsBetweenPredicate().Args[1].AsNumeric().Value);
            Assert.Equal(50.0, be.AsComparison().AsIsBetweenPredicate().Args[2].AsNumeric().Value);

            var json2 = JsonConvert.SerializeObject(new CQL2Expression(be), _settings);
            JsonAssert.AreEqual(json, json2);
        }

        [Fact]
        public async Task Example11Test()
        {
            var json = GetJson("CQL2", "Example11");
            JObject jObject = JObject.Parse(json);
            var be = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            Assert.IsType<IsLikePredicate>(be);
            Assert.NotNull(be.AsComparison().AsIsLikePredicate());
            Assert.Equal(IsLikePredicateOp.Like, be.AsComparison().AsIsLikePredicate().Op);
            Assert.Equal(2, be.AsComparison().AsIsLikePredicate().Args.Count);

            Assert.Equal("mission", be.AsComparison().AsIsLikePredicate().Args[0].AsPropertyRef().Property);
            Assert.IsType<Models.Cql2.String>(be.AsComparison().AsIsLikePredicate().Args[1]);
            Assert.Equal("sentinel%", be.AsComparison().AsIsLikePredicate().Args[1].ToString());

            var json2 = JsonConvert.SerializeObject(new CQL2Expression(be), _settings);
            JsonAssert.AreEqual(json, json2);
        }

        [Fact]
        public async Task Example12Test()
        {
            var json = GetJson("CQL2", "Example12");
            JObject jObject = JObject.Parse(json);
            var be = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            Assert.IsType<IsInListPredicate>(be);
            Assert.NotNull(be.AsComparison().AsIsInListPredicate());
            Assert.Equal(IsInListPredicateOp.In, be.AsComparison().AsIsInListPredicate().Op);
            Assert.Equal(2, be.AsComparison().AsIsInListPredicate().Args.Count);

            Assert.Equal("keywords", be.AsComparison().AsIsInListPredicate().Args[0].AsScalarExpression().AsCharExpression().AsPropertyRef().Property);
            Assert.IsType<ScalarExpressionCollection>(be.AsComparison().AsIsInListPredicate().Args[1]);
            Assert.Equal<string>(new List<string>(){ "fire", "forest", "wildfire" }, be.AsComparison().AsIsInListPredicate().Args[1].AsScalarExpressionCollection().Select(x => x.AsCharExpression().ToString()).ToList());

            var json2 = JsonConvert.SerializeObject(new CQL2Expression(be), _settings);
            JsonAssert.AreEqual(json, json2);
        }
    }
}
