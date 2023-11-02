using GeoJSON.Net.Geometry;
using Stac.Api.Attributes;
using Stac.Api.Clients.ItemSearch;
using Stac.Api.Models;

namespace Stac.Api.WebApi.Controllers.ItemSearch
{
    [ConformanceClass("https://api.stacspec.org/v1.0.0/item-search")]
    [LandingPageAction("GetItemSearch", "search", StacItem.MEDIATYPE, Method = "GET")]
    [LandingPageAction("PostItemSearch", "search", StacItem.MEDIATYPE, Method = "POST")]
    public partial class ItemSearchController : Stac.Api.WebApi.StacApiController
    {
        public override object GetActionParameters(string actionName)
        {
            if (actionName == "GetItemSearch")
            {
                return new { intersects = default(IGeometryObject) };
            }
            else if (actionName == "PostItemSearch")
            {
                return new { body = default(SearchBody) };
            }

            return null;

        }
    }

}
