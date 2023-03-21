using Xunit;

namespace Stac.Api.Tests
{
    [CollectionDefinition(Name)]
    public sealed class StacApiAppCollectionFixture : ICollectionFixture<TestCatalogsProvider>
    {
        public const string Name = "Stac API App server collection";
    }
}