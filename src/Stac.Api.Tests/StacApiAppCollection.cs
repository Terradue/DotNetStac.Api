using Xunit;

namespace Stac.Api.Tests
{
    [CollectionDefinition(Name)]
    public sealed class StacApiAppCollection : ICollectionFixture<StacApiAppFixture>
    {
        public const string Name = "Stac API App server collection";
    }
}