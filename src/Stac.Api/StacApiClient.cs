using System.Net.Http;

namespace Stac.Api
{
    public abstract class StacApiClient
    {
        private string _baseUrl;

        protected StacApiClient()
        {
            BaseUrl = null;
        }

        protected StacApiClient(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public string BaseUrl
        {
            get { return _baseUrl; }
            internal set { _baseUrl = value; }
        }
    }
}