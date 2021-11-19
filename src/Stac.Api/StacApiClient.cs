namespace Stac.Api
{
    public abstract class StacApiClient
    {
        private string _baseUrl;

        protected StacApiClient()
        {
        }

        public string BaseUrl
        {
            get { return _baseUrl; }
            internal set { _baseUrl = value; }
        }
    }
}