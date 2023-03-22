using System;
using System.Net.Http;
using Stac.Api.Clients.ItemSearch;

namespace Stac.Api.Clients
{
    public sealed partial class ApiClient
    {
        private ItemSearchClient _itemSearch;

        public ItemSearchClient ItemSearch
        {
            get
            {
                if (_itemSearch == null)
                {
                    _itemSearch = new ItemSearchClient(_client);
                }
                return _itemSearch;
            }
        }
    }
}