
namespace Stac.Api.WebApi.Services
{
    public interface IStacLinkValuesProvider<T> where T : ILinksCollectionObject
    {
        IEnumerable<ILinkValues> GetLinkValues();
    }
}