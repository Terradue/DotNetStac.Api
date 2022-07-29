using System.Reflection;

namespace Stac.Api.WebApi.Implementations
{
    public class DefaultBaseController
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public Uri AppBaseUrl => new Uri($"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}");

        public DefaultBaseController(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

         private static readonly Assembly ThisAssembly = typeof(DefaultBaseController).Assembly;

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = ThisAssembly.CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        protected IEnumerable<StacCollection> GetCollections(string path)
        {
            var files = Directory.GetFiles(Path.Combine(AssemblyDirectory, path));
            foreach (var file in files)
            {
                var collection = StacConvert.Deserialize<StacCollection>(File.ReadAllText(file));
                yield return collection;
            }
        }
    }
}