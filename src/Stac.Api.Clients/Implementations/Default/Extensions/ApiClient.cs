using System;
using System.Net.Http;
using Stac.Api.Clients.Extensions;

namespace Stac.Api.Clients
{
    public sealed partial class ApiClient
    {
        private ExtensionsClient _extensions;

        public ExtensionsClient Extensions
        {
            get
            {
                if (_extensions == null)
                {
                    _extensions = new ExtensionsClient(_client);
                }
                return _extensions;
            }
        }
    }
}