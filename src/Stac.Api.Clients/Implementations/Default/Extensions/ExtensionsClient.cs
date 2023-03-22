using System.Net.Http;

namespace Stac.Api.Clients.Extensions
{
    public partial class ExtensionsClient
    {
        private HttpClient _client;

        public ExtensionsClient(HttpClient client)
        {
            _client = client;
        }

    }
}