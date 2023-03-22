using System;
using System.Net.Http;
using Stac.Api.Clients.Core;

namespace Stac.Api.Clients
{
    public sealed partial class ApiClient
    {
        private CoreClient _core;

        public CoreClient Core
        {
            get
            {
                if (_core == null)
                {
                    _core = new CoreClient(_client);
                }
                return _core;
            }
        }
    }
}