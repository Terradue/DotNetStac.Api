using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GeoJSON.Net.Geometry;
using Stac.Api.Clients.Features;
using Stac.Api.Clients.ItemSearch;
using Stac.Api.Models;

namespace Stac.Api.Clients.Fluent
{
    public class ItemSearch
    {
        private ItemSearchClient _itemSearchClient;

        private SearchBody _body;

        public ItemSearch(ItemSearchClient itemSearchClient)
        {
            _itemSearchClient = itemSearchClient;
            _body = new SearchBody();
        }

        public async Task<StacFeatureCollection> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            return await _itemSearchClient.PostItemSearchAsync(_body, cancellationToken);
        }
        
        public ItemSearch WithBbox(double minX, double minY, double maxX, double maxY)
        {
            _body.Bbox = new Bbox(){ minX, minY, maxX, maxY };
            return this;
        }

        public ItemSearch WithDatetime(DateTime? start, DateTime? end = null)
        {
            // Single date+time, or a range ('/' separator), formatted to RFC 3339, section 5.6. Use double dots .. for open date ranges.
            string datetime = null;
            if (start.HasValue && end.HasValue)
            {
                datetime = $"{start.Value:yyyy-MM-ddTHH:mm:ssZ}/{end.Value:yyyy-MM-ddTHH:mm:ssZ}";
            }
            else if (start.HasValue)
            {
                datetime = $"{start.Value:yyyy-MM-ddTHH:mm:ssZ}/..";
            }
            else if (end.HasValue)
            {
                datetime = $"../{end.Value:yyyy-MM-ddTHH:mm:ssZ}";
            }
            _body.Datetime = datetime;
            return this;
        }

        public ItemSearch WithIds(IEnumerable<string> ids)
        {
            _body.Ids = new Ids(ids);
            return this;
        }

        public ItemSearch From(string collections)
        {
            // Add the collection to the existings if not empty
            _body.Collections = _body.Collections?.Append(collections).ToArray() ?? new[] { collections };
            return this;
        }

        public ItemSearch From(IEnumerable<string> collections)
        {
            _body.Collections = collections.ToArray();
            return this;
        }

        public ItemSearch Intersects(IGeometryObject intersects)
        {
            _body.Intersects = new Models.Core.IntersectGeometryFilter(intersects);
            return this;
        }

        public ItemSearch Limit(int limit)
        {
            _body.Limit = limit;
            return this;
        }

    }
}