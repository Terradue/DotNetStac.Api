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
using Stac.Api.Services.Filtering;
using Microsoft.AspNetCore.Http;
using HttpContextMoq;
using HttpContextMoq.Extensions;
using System.Linq.Expressions;
using System.Linq;
using Stac.Api.Services.Queryable;
using Stac.Api.WebApi.Implementations.Default.Services;
using Stac.Api.Interfaces;

namespace Stac.Api.Tests
{
    public class CQL2FilteringTests : TestBase
    {
        private readonly JsonSerializerSettings _settings;

        public CQL2FilteringTests(ITestOutputHelper outputHelper) : base(outputHelper)
        {
            _settings = new JsonSerializerSettings();
            _settings.Converters.Add(new BooleanExpressionConverter());
        }

        [Fact]
        public async Task Example0Test()
        {
            var json = GetJson("CQL2", "Example0", "CQL2Tests");
            JObject jObject = JObject.Parse(json);
            var cql = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            json = GetJson("CQL2", "SampleItem", "CQL2Tests");
            StacItem item = JsonConvert.DeserializeObject<StacItem>(json);
            IQueryable<StacItem> source = new StacItem[] { item }.AsQueryable();
            var provider = DefaultStacQueryProvider.CreateDefaultQueryProvider(TestStacApiContext, source);
            StacQueryable<StacItem> source2 = new StacQueryable<StacItem>(provider, source.Expression);
            source2 = source2.Boolean(cql) as StacQueryable<StacItem>;
            OutputHelper.WriteLine(source2.Expression.ToString());
            Assert.Equal(0, source2.Count());
        }

        [Fact]
        public async Task Example1Test()
        {
            var json = GetJson("CQL2", "Example1", "CQL2Tests");
            JObject jObject = JObject.Parse(json);
            var cql = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            json = GetJson("CQL2", "SampleItem", "CQL2Tests");
            StacItem item = JsonConvert.DeserializeObject<StacItem>(json);
            IQueryable<StacItem> source = new StacItem[] { item }.AsQueryable();
            var provider = DefaultStacQueryProvider.CreateDefaultQueryProvider(TestStacApiContext, source);
            StacQueryable<StacItem> source2 = new StacQueryable<StacItem>(provider, source.Expression);
            source2 = source2.Boolean(cql) as StacQueryable<StacItem>;
            OutputHelper.WriteLine(source2.Expression.ToString());
            Assert.Equal(1, source2.Count());
        }

        [Fact]
        public async Task Example2Test()
        {
            var json = GetJson("CQL2", "Example2", "CQL2Tests");
            JObject jObject = JObject.Parse(json);
            var cql = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            json = GetJson("CQL2", "SampleItem", "CQL2Tests");
            StacItem item = JsonConvert.DeserializeObject<StacItem>(json);
            IQueryable<StacItem> source = new StacItem[] { item }.AsQueryable();
            var provider = DefaultStacQueryProvider.CreateDefaultQueryProvider(TestStacApiContext, source);
            StacQueryable<StacItem> source2 = new StacQueryable<StacItem>(provider, source.Expression);
            source2 = source2.Boolean(cql) as StacQueryable<StacItem>;
            OutputHelper.WriteLine(source2.Expression.ToString());
            Assert.Equal(0, source2.Count());
        }

        [Fact]
        public async Task Example3Test()
        {
            var json = GetJson("CQL2", "Example3", "CQL2Tests");
            JObject jObject = JObject.Parse(json);
            var cql = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            json = GetJson("CQL2", "SampleItem", "CQL2Tests");
            StacItem item = JsonConvert.DeserializeObject<StacItem>(json);
            IQueryable<StacItem> source = new StacItem[] { item }.AsQueryable();
            var provider = DefaultStacQueryProvider.CreateDefaultQueryProvider(TestStacApiContext, source);
            StacQueryable<StacItem> source2 = new StacQueryable<StacItem>(provider, source.Expression);
            source2 = source2.Boolean(cql) as StacQueryable<StacItem>;
            OutputHelper.WriteLine(source2.Expression.ToString());
            Assert.Equal(0, source2.Count());
        }

        [Fact]
        public async Task Example4Test()
        {
            var json = GetJson("CQL2", "Example4", "CQL2Tests");
            JObject jObject = JObject.Parse(json);
            var cql = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            json = GetJson("CQL2", "SampleItem", "CQL2Tests");
            StacItem item = JsonConvert.DeserializeObject<StacItem>(json);
            IQueryable<StacItem> source = new StacItem[] { item }.AsQueryable();
            var provider = DefaultStacQueryProvider.CreateDefaultQueryProvider(TestStacApiContext, source);
            StacQueryable<StacItem> source2 = new StacQueryable<StacItem>(provider, source.Expression);
            source2 = source2.Boolean(cql) as StacQueryable<StacItem>;
            OutputHelper.WriteLine(source2.Expression.ToString());
            Assert.Equal(1, source2.Count());
        }

        [Fact]
        public async Task Example5Test()
        {
            var json = GetJson("CQL2", "Example5", "CQL2Tests");
            JObject jObject = JObject.Parse(json);
            var cql = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            json = GetJson("CQL2", "SampleItem", "CQL2Tests");
            StacItem item = JsonConvert.DeserializeObject<StacItem>(json);
            IQueryable<StacItem> source = new StacItem[] { item }.AsQueryable();
            var provider = DefaultStacQueryProvider.CreateDefaultQueryProvider(TestStacApiContext, source);
            StacQueryable<StacItem> source2 = new StacQueryable<StacItem>(provider, source.Expression);
            source2 = source2.Boolean(cql) as StacQueryable<StacItem>;
            OutputHelper.WriteLine(source2.Expression.ToString());
            Assert.Equal(0, source2.Count());
        }

        [Fact]
        public async Task Example6Test()
        {
            var json = GetJson("CQL2", "Example6", "CQL2Tests");
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

            json = GetJson("CQL2", "SampleItem", "CQL2Tests");
            StacItem item = JsonConvert.DeserializeObject<StacItem>(json);
            IQueryable<StacItem> source = new StacItem[] { item }.AsQueryable();
            var provider = DefaultStacQueryProvider.CreateDefaultQueryProvider(TestStacApiContext, source);
            StacQueryable<StacItem> source2 = new StacQueryable<StacItem>(provider, source.Expression);
            source2 = source2.Boolean(cql) as StacQueryable<StacItem>;
            OutputHelper.WriteLine(source2.Expression.ToString());
            Assert.Equal(1, source2.Count());

        }

        [Fact]
        public async Task Example7Test()
        {
            var json = GetJson("CQL2", "Example7", "CQL2Tests");
            JObject jObject = JObject.Parse(json);
            var cql = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            Assert.IsType<SpatialPredicate>(cql);
            Assert.NotNull(cql.AsComparison().AsSpatialPredicate());
            Assert.Equal(SpatialPredicateOp.S_intersects, cql.AsComparison().AsSpatialPredicate().Op);
            Assert.Equal(2, cql.AsComparison().AsSpatialPredicate().Args.Count);

            Assert.Equal("geometry", cql.AsComparison().AsSpatialPredicate().Args[0].AsPropertyRef().Property);
            Assert.IsType<Polygon>(cql.AsComparison().AsSpatialPredicate().Args[1].AsSpatialLiteral().AsGeometryLiteral().GeometryObject);

            json = GetJson("CQL2", "SampleItem", "CQL2Tests");
            StacItem item = JsonConvert.DeserializeObject<StacItem>(json);
            IQueryable<StacItem> source = new StacItem[] { item }.AsQueryable();
            var provider = DefaultStacQueryProvider.CreateDefaultQueryProvider(TestStacApiContext, source);
            StacQueryable<StacItem> source2 = new StacQueryable<StacItem>(provider, source.Expression);
            source2 = source2.Boolean(cql) as StacQueryable<StacItem>;
            OutputHelper.WriteLine(source2.Expression.ToString());
            Assert.Equal(0, source2.Count());

        }

        [Fact]
        public async Task Example8Test()
        {
            var json = GetJson("CQL2", "Example8", "CQL2Tests");
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

            json = GetJson("CQL2", "SampleItem", "CQL2Tests");
            StacItem item = JsonConvert.DeserializeObject<StacItem>(json);
            IQueryable<StacItem> source = new StacItem[] { item }.AsQueryable();
            var provider = DefaultStacQueryProvider.CreateDefaultQueryProvider(TestStacApiContext, source);
            StacQueryable<StacItem> source2 = new StacQueryable<StacItem>(provider, source.Expression);
            source2 = source2.Boolean(cql) as StacQueryable<StacItem>;
            OutputHelper.WriteLine(source2.Expression.ToString());
            Assert.Equal(0, source2.Count());

        }

        [Fact]
        public async Task Example9Test()
        {
            var json = GetJson("CQL2", "Example9", "CQL2Tests");
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

            json = GetJson("CQL2", "SampleItem", "CQL2Tests");
            StacItem item = JsonConvert.DeserializeObject<StacItem>(json);
            IQueryable<StacItem> source = new StacItem[] { item }.AsQueryable();
            var provider = DefaultStacQueryProvider.CreateDefaultQueryProvider(TestStacApiContext, source);
            StacQueryable<StacItem> source2 = new StacQueryable<StacItem>(provider, source.Expression);
            source2 = source2.Boolean(cql) as StacQueryable<StacItem>;
            OutputHelper.WriteLine(source2.Expression.ToString());
            Assert.Equal(1, source2.Count());

        }

        [Fact]
        public async Task Example10Test()
        {
            var json = GetJson("CQL2", "Example10", "CQL2Tests");
            JObject jObject = JObject.Parse(json);
            var cql = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            Assert.IsType<IsBetweenPredicate>(cql);
            Assert.NotNull(cql.AsComparison().AsIsBetweenPredicate());
            Assert.Equal(IsBetweenPredicateOp.Between, cql.AsComparison().AsIsBetweenPredicate().Op);
            Assert.Equal(3, cql.AsComparison().AsIsBetweenPredicate().Args.Count);

            Assert.Equal("eo:cloud_cover", cql.AsComparison().AsIsBetweenPredicate().Args[0].AsCharExpression().AsPropertyRef().Property);
            Assert.Equal((double)0, cql.AsComparison().AsIsBetweenPredicate().Args[1].AsNumeric().Value);
            Assert.Equal((double)50, cql.AsComparison().AsIsBetweenPredicate().Args[2].AsNumeric().Value);

            json = GetJson("CQL2", "SampleItem", "CQL2Tests");
            StacItem item = JsonConvert.DeserializeObject<StacItem>(json);
            IQueryable<StacItem> source = new StacItem[] { item }.AsQueryable();
            var provider = DefaultStacQueryProvider.CreateDefaultQueryProvider(TestStacApiContext, source);
            StacQueryable<StacItem> source2 = new StacQueryable<StacItem>(provider, source.Expression);
            source2 = source2.Boolean(cql) as StacQueryable<StacItem>;
            OutputHelper.WriteLine(source2.Expression.ToString());
            Assert.Equal(0, source2.Count());

        }

        [Fact]
        public async Task Example11Test()
        {
            var json = GetJson("CQL2", "Example11", "CQL2Tests");
            JObject jObject = JObject.Parse(json);
            var cql = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            Assert.IsType<IsLikePredicate>(cql);
            Assert.NotNull(cql.AsComparison().AsIsLikePredicate());
            Assert.Equal(IsLikePredicateOp.Like, cql.AsComparison().AsIsLikePredicate().Op);
            Assert.Equal(2, cql.AsComparison().AsIsLikePredicate().Args.Count);

            Assert.Equal("mission", cql.AsComparison().AsIsLikePredicate().Args[0].AsPropertyRef().Property);
            Assert.IsType<Models.Cql2.String>(cql.AsComparison().AsIsLikePredicate().Args[1]);
            Assert.Equal("sentinel%", cql.AsComparison().AsIsLikePredicate().Args[1].ToString());

            json = GetJson("CQL2", "SampleItem", "CQL2Tests");
            StacItem item = JsonConvert.DeserializeObject<StacItem>(json);
            IQueryable<StacItem> source = new StacItem[] { item }.AsQueryable();
            var provider = DefaultStacQueryProvider.CreateDefaultQueryProvider(TestStacApiContext, source);
            StacQueryable<StacItem> source2 = new StacQueryable<StacItem>(provider, source.Expression);
            source2 = source2.Boolean(cql) as StacQueryable<StacItem>;
            OutputHelper.WriteLine(source2.Expression.ToString());
            Assert.Equal(0, source2.Count());

        }
    }
}
