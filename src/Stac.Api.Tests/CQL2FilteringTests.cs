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
            json = GetJson("CQL2", "SampleItem");
            StacItem item = JsonConvert.DeserializeObject<StacItem>(json);
            IQueryable<StacItem> source = new StacItem[] { item }.AsQueryable();
            var provider = DefaultStacQueryProvider.CreateDefaultQueryProvider(new HttpContextMock().SetupUrl("http://localhost:5000"), source);
            StacQueryable<StacItem> source2 = new StacQueryable<StacItem>(provider, source.Expression);
            source2 = source2.WithCQL2(cql);
            OutputHelper.WriteLine(source2.Expression.ToString());
            Assert.Equal(0, source2.Count());
        }

        [Fact]
        public async Task Example1Test()
        {
            var json = GetJson("CQL2", "Example1", "CQL2Tests");
            JObject jObject = JObject.Parse(json);
            var cql = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            json = GetJson("CQL2", "SampleItem");
            StacItem item = JsonConvert.DeserializeObject<StacItem>(json);
            IQueryable<StacItem> source = new StacItem[] { item }.AsQueryable();
            var provider = DefaultStacQueryProvider.CreateDefaultQueryProvider(new HttpContextMock().SetupUrl("http://localhost:5000"), source);
            StacQueryable<StacItem> source2 = new StacQueryable<StacItem>(provider, source.Expression);
            source2 = source2.WithCQL2(cql);
            OutputHelper.WriteLine(source2.Expression.ToString());
            Assert.Equal(1, source2.Count());
        }

        [Fact]
        public async Task Example2Test()
        {
            var json = GetJson("CQL2", "Example2", "CQL2Tests");
            JObject jObject = JObject.Parse(json);
            var cql = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            json = GetJson("CQL2", "SampleItem");
            StacItem item = JsonConvert.DeserializeObject<StacItem>(json);
            IQueryable<StacItem> source = new StacItem[] { item }.AsQueryable();
            var provider = DefaultStacQueryProvider.CreateDefaultQueryProvider(new HttpContextMock().SetupUrl("http://localhost:5000"), source);
            StacQueryable<StacItem> source2 = new StacQueryable<StacItem>(provider, source.Expression);
            source2 = source2.WithCQL2(cql);
            OutputHelper.WriteLine(source2.Expression.ToString());
        }

        [Fact]
        public async Task Example3Test()
        {
            var json = GetJson("CQL2", "Example3", "CQL2Tests");
            JObject jObject = JObject.Parse(json);
            var cql = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            json = GetJson("CQL2", "SampleItem");
            StacItem item = JsonConvert.DeserializeObject<StacItem>(json);
            IQueryable<StacItem> source = new StacItem[] { item }.AsQueryable();
            var provider = DefaultStacQueryProvider.CreateDefaultQueryProvider(new HttpContextMock().SetupUrl("http://localhost:5000"), source);
            StacQueryable<StacItem> source2 = new StacQueryable<StacItem>(provider, source.Expression);
            source2 = source2.WithCQL2(cql);
            OutputHelper.WriteLine(source2.Expression.ToString());
        }

        [Fact]
        public async Task Example4Test()
        {
            var json = GetJson("CQL2", "Example4", "CQL2Tests");
            JObject jObject = JObject.Parse(json);
            var cql = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            json = GetJson("CQL2", "SampleItem");
            StacItem item = JsonConvert.DeserializeObject<StacItem>(json);
            // Assert.True(item.Filter(cql));
        }

        [Fact]
        public async Task Example5Test()
        {
            var json = GetJson("CQL2", "Example5", "CQL2Tests");
            JObject jObject = JObject.Parse(json);
            var cql = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            json = GetJson("CQL2", "SampleItem");
            StacItem item = JsonConvert.DeserializeObject<StacItem>(json);
            // Assert.True(item.Filter(cql));
        }

        [Fact]
        public async Task Example6Test()
        {
            var json = GetJson("CQL2", "Example6");
            JObject jObject = JObject.Parse(json);
            var cql = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            Assert.IsType<TemporalPredicate>(cql);
            Assert.NotNull(cql.Comparison().Temporal());
            Assert.Equal(TemporalPredicateOp.T_intersects, cql.Comparison().Temporal().Op);
            Assert.Equal(2, cql.Comparison().Temporal().Args.Count);

            Assert.Equal("datetime", cql.Comparison().Temporal().Args[0].Property().Property);
            var date1 = DateTimeOffset.Parse("2020-11-11T00:00:00Z", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            var date2 = DateTimeOffset.Parse("2020-11-12T00:00:00Z", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            Itenso.TimePeriod.TimeInterval interval = new Itenso.TimePeriod.TimeInterval(date1.Date, date2.Date);
            Assert.Equal(interval, cql.Comparison().Temporal().Args[1].TemporalLiteral().IntervalLiteral().TimeInterval);

        }

        [Fact]
        public async Task Example7Test()
        {
            var json = GetJson("CQL2", "Example7");
            JObject jObject = JObject.Parse(json);
            var cql = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            Assert.IsType<SpatialPredicate>(cql);
            Assert.NotNull(cql.Comparison().Spatial());
            Assert.Equal(SpatialPredicateOp.S_intersects, cql.Comparison().Spatial().Op);
            Assert.Equal(2, cql.Comparison().Spatial().Args.Count);

            Assert.Equal("geometry", cql.Comparison().Spatial().Args[0].Property().Property);
            Assert.IsType<Polygon>(cql.Comparison().Spatial().Args[1].SpatialLiteral().GeometryLiteral().GeometryObject);

        }

        [Fact]
        public async Task Example8Test()
        {
            var json = GetJson("CQL2", "Example8");
            JObject jObject = JObject.Parse(json);
            var cql = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            Assert.IsType<AndOrExpression>(cql);
            Assert.NotNull(cql.AndOr());
            Assert.Equal(AndOrExpressionOp.Or, cql.AndOr().Op);
            Assert.Equal(2, cql.AndOr().Args.Count);

            Assert.Equal(SpatialPredicateOp.S_intersects, cql.AndOr().Args[0].Comparison().Spatial().Op);
            Assert.Equal("geometry", cql.AndOr().Args[0].Comparison().Spatial().Args[0].Property().Property);
            Assert.IsType<Polygon>(cql.AndOr().Args[0].Comparison().Spatial().Args[1].SpatialLiteral().GeometryLiteral().GeometryObject);

            Assert.Equal(SpatialPredicateOp.S_intersects, cql.AndOr().Args[1].Comparison().Spatial().Op);
            Assert.Equal("geometry", cql.AndOr().Args[1].Comparison().Spatial().Args[0].Property().Property);
            Assert.IsType<Polygon>(cql.AndOr().Args[1].Comparison().Spatial().Args[1].SpatialLiteral().GeometryLiteral().GeometryObject);

        }

        [Fact]
        public async Task Example9Test()
        {
            var json = GetJson("CQL2", "Example9", "CQL2Tests");
            JObject jObject = JObject.Parse(json);
            var cql = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            Assert.IsType<AndOrExpression>(cql);
            Assert.NotNull(cql.AndOr());
            Assert.Equal(AndOrExpressionOp.Or, cql.AndOr().Op);
            Assert.Equal(3, cql.AndOr().Args.Count);

            Assert.Equal(ComparisonPredicateOp.Gt, cql.AndOr().Args[0].Comparison().Binary().Op);
            Assert.Equal("sentinel:data_coverage", cql.AndOr().Args[0].Comparison().Binary().Args[0].Char().Property().Property);
            Assert.Equal(50, cql.AndOr().Args[0].Comparison().Binary().Args[1].Numeric().AsNumber().Num);

            Assert.Equal(ComparisonPredicateOp.Lt, cql.AndOr().Args[1].Comparison().Binary().Op);
            Assert.Equal("landsat:coverage_percent", cql.AndOr().Args[1].Comparison().Binary().Args[0].Char().Property().Property);
            Assert.Equal(10, cql.AndOr().Args[1].Comparison().Binary().Args[1].Numeric().AsNumber().Num);

            Assert.Equal(AndOrExpressionOp.And, cql.AndOr().Args[2].AndOr().Op);
            Assert.IsType<IsNullPredicate>(cql.AndOr().Args[2].AndOr().Args[0]);
            Assert.IsType<IsNullPredicate>(cql.AndOr().Args[2].AndOr().Args[1]);
            Assert.Equal("sentinel:data_coverage", cql.AndOr().Args[2].AndOr().Args[0].Comparison().IsNull().Args.Char().Property().Property);
            Assert.Equal("landsat:coverage_percent", cql.AndOr().Args[2].AndOr().Args[1].Comparison().IsNull().Args.Char().Property().Property);

        }

        [Fact]
        public async Task Example10Test()
        {
            var json = GetJson("CQL2", "Example10", "CQL2Tests");
            JObject jObject = JObject.Parse(json);
            var cql = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            Assert.IsType<IsBetweenPredicate>(cql);
            Assert.NotNull(cql.Comparison().IsBetween());
            Assert.Equal(IsBetweenPredicateOp.Between, cql.Comparison().IsBetween().Op);
            Assert.Equal(3, cql.Comparison().IsBetween().Args.Count);

            Assert.Equal("eo:cloud_cover", cql.Comparison().IsBetween().Args[0].Char().Property().Property);
            Assert.Equal((double)0, cql.Comparison().IsBetween().Args[1].Numeric().Value);
            Assert.Equal((double)50, cql.Comparison().IsBetween().Args[2].Numeric().Value);

            json = GetJson("CQL2", "SampleItem");
            StacItem item = JsonConvert.DeserializeObject<StacItem>(json);
            IQueryable<StacItem> source = new StacItem[] { item }.AsQueryable();
            var provider = DefaultStacQueryProvider.CreateDefaultQueryProvider(new HttpContextMock().SetupUrl("http://localhost:5000"), source);
            StacQueryable<StacItem> source2 = new StacQueryable<StacItem>(provider, source.Expression);
            source2 = source2.WithCQL2(cql);
            OutputHelper.WriteLine(source2.Expression.ToString());

        }

        [Fact]
        public async Task Example11Test()
        {
            var json = GetJson("CQL2", "Example11", "CQL2Tests");
            JObject jObject = JObject.Parse(json);
            var cql = JsonConvert.DeserializeObject<BooleanExpression>(jObject["filter"].ToString(), _settings);
            Assert.IsType<IsLikePredicate>(cql);
            Assert.NotNull(cql.Comparison().IsLike());
            Assert.Equal(IsLikePredicateOp.Like, cql.Comparison().IsLike().Op);
            Assert.Equal(2, cql.Comparison().IsLike().Args.Count);

            Assert.Equal("mission", cql.Comparison().IsLike().Args[0].Property().Property);
            Assert.IsType<Models.Cql2.String>(cql.Comparison().IsLike().Args[1]);
            Assert.Equal("sentinel%", cql.Comparison().IsLike().Args[1].ToString());

            json = GetJson("CQL2", "SampleItem");
            StacItem item = JsonConvert.DeserializeObject<StacItem>(json);
            IQueryable<StacItem> source = new StacItem[] { item }.AsQueryable();
            var provider = DefaultStacQueryProvider.CreateDefaultQueryProvider(new HttpContextMock().SetupUrl("http://localhost:5000"), source);
            StacQueryable<StacItem> source2 = new StacQueryable<StacItem>(provider, source.Expression);
            source2 = source2.WithCQL2(cql);
            OutputHelper.WriteLine(source2.Expression.ToString());

        }
    }
}
