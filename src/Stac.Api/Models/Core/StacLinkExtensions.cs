using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stac.Api.Models;

namespace Stac.Api.Models.Core
{
    public static class StacLinkExtensions
    {
        public static StacLink NextPage(this ILinksCollectionObject linksCollectionObject)
        {
            return linksCollectionObject.Links.FirstOrDefault(l => l.RelationshipType == "next");
        }

        public static StacLink PreviousPage(this ILinksCollectionObject linksCollectionObject)
        {
            return linksCollectionObject.Links.FirstOrDefault(l => l.RelationshipType == "prev");
        }
    }
}