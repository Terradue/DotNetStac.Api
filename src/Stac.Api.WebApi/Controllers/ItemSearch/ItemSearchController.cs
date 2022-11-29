using Stac.Api.Attributes;
using Stac.Api.Models;

namespace Stac.Api.WebApi.Controllers.ItemSearch
{
    [ConformanceClass("https://api.stacspec.org/v1.0.0-rc.2/item-search")]
    [LandingPageAction("GetItemSearch", "search", StacItem.MEDIATYPE, Method = "GET")]
    [LandingPageAction("PostItemSearch", "search", StacItem.MEDIATYPE, Method = "POST")]
    public partial class ItemSearchController : Stac.Api.WebApi.StacApiController
    {
    }

}
