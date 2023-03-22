using System.Net.Http;
using Stac.Api.Clients.Extensions.Filter;

namespace Stac.Api.Clients.Extensions
{
    public partial class ExtensionsClient
    {
        private FilterClient _filter;

        public FilterClient Filter
        {
            get
            {
                if (_filter == null)
                {
                    _filter = new FilterClient(_client);
                }
                return _filter;
            }
        }

    }
}