using System;
using System.Net.Http;
using Stac.Api.Clients.Features;

namespace Stac.Api.Clients
{
    public sealed partial class ApiClient
    {
        private FeaturesClient _features;

        public FeaturesClient Features
        {
            get
            {
                if (_features == null)
                {
                    _features = new FeaturesClient(_client);
                }
                return _features;
            }
        }
    }
}