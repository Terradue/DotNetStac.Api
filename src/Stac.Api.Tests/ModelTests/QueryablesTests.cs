using Xunit;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Api.Converters;
using Newtonsoft.Json.Schema;
using NJsonSchema;
using Stac.Api.Models;

namespace Stac.Api.Tests
{
    public class QueryablesTests : TestBase
    {
        private readonly JsonSerializerSettings _settings;

        public QueryablesTests(ITestOutputHelper outputHelper) : base(outputHelper)
        {
            _settings = new JsonSerializerSettings();
            _settings.Converters.Add(new BooleanExpressionConverter());
        }

        [Fact]
        public async Task Example1Test()
        {
            var schema = new StacQueryables()
            {
                SchemaVersion = new System.Uri("https://json-schema.org/draft/2019-09/schema"),
                Id = new System.Uri("http://localhost:5000/queryables").ToString(),
                Type = "object",
                Title = "STAC Item Queryables",
                Description = "Queryable names for STAC Items",
                Properties = {
                    {
                        "id", new StacQueryablesProperty()
                        {
                            Reference = new System.Uri("https://schemas.stacspec.org/v1.0.0/item-spec/json-schema/item.json#/id"),
                            Description = "The STAC Item ID"
                        }
                    }
                },
            };

            var json = JsonConvert.SerializeObject(schema, _settings);
            JObject o = JObject.Parse(json);
            Assert.NotNull(o["properties"]["id"]);

        }

    }
}
