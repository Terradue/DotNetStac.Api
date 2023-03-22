using System;
using System.Net.Http;
using Stac.Api.Clients.Collections;

namespace Stac.Api.Clients
{
    public sealed partial class ApiClient
    {
        private CollectionsClient _collections;

        public CollectionsClient Collections
        {
            get
            {
                if (_collections == null)
                {
                    _collections = new CollectionsClient(_client);
                }
                return _collections;
            }
        }
    }
}