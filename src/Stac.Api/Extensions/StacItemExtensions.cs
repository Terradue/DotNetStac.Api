using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoJSON.Net.Geometry;
using Stac.Api.Models;

namespace Stac.Api.Extensions
{
    public static class StacItemExtensions
    {
        public static StacItem Patch(this StacItem item, PatchStacItem patch)
        {
            IGeometryObject geometry = item.Geometry;
            if ( patch.Geometry != null )
            {
                geometry = patch.Geometry;
            }
            var properties = item.Properties;
            foreach ( var property in patch.Properties )
            {
                properties.Remove(property.Key);
                properties.Add(property.Key, property.Value);
            }
            var assets = item.Assets;
            foreach ( var asset in patch.Assets )
            {
                assets.Remove(asset.Key);
                assets.Add(asset.Key, asset.Value);
            }
            var links = item.Links;
            foreach ( var link in patch.Links )
            {
                var linkFound = links.FirstOrDefault(l => l.Uri == link.Uri);
                if ( linkFound != null )
                {
                    links.Remove(linkFound);
                }
                links.Add(link);
            }
            var newItem = new StacItem(item.Id, geometry, properties);
            newItem.Assets.AddRange(assets);
            newItem.Links.AddRange(links);

            return newItem;
        }
    }
}