using System;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Xunit.DependencyInjection;

namespace Stac.Api.Tests
{
    public class StacApiAppFixture : WebApplicationFactory<Program>, ITestOutputHelperAccessor
    {
        public StacApiAppFixture()
        {
            // Use HTTPS by default and do not follow
            // redirects so they can tested explicitly.
            ClientOptions.AllowAutoRedirect = false;
            ClientOptions.BaseAddress = new Uri("https://localhost");

            // Configure HTTP requests that are not intercepted by
            // the tests to throw an exception to cause it to fail.
            Interceptor = new HttpClientInterceptorOptions().ThrowsOnMissingRegistration();
        }

        public HttpClientInterceptorOptions Interceptor { get; }

        public ITestOutputHelper? OutputHelper { get; set; }

        public void ClearOutputHelper()
            => OutputHelper = null;

        public void SetOutputHelper(ITestOutputHelper value)
            => OutputHelper = value;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(configBuilder =>
            {
                // Configure the test fixture to write the SQLite database
                // to a temporary directory, rather than in App_Data.
                var dataDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

                if (!Directory.Exists(dataDirectory))
                {
                    Directory.CreateDirectory(dataDirectory);
                }

                // Also override the default options for the GitHub OAuth provider
                var config = new[]
                {
                KeyValuePair.Create("DataDirectory", dataDirectory),
                KeyValuePair.Create("GitHub:ClientId", "github-id"),
                KeyValuePair.Create("GitHub:ClientSecret", "github-secret"),
                KeyValuePair.Create("GitHub:EnterpriseDomain", string.Empty)
                };

                configBuilder.AddInMemoryCollection(config);
            });

            // Route the application's logs to the xunit output
            builder.ConfigureLogging(loggingBuilder => loggingBuilder.ClearProviders().AddXUnit(this));

            // Configure the correct content root for the static content and Razor pages
            builder.UseSolutionRelativeContentRoot(Path.Combine("src", "TodoApp"));

            // Configure the application so HTTP requests related to the OAuth flow
            // can be intercepted and redirected to not use the real GitHub service.
            builder.ConfigureServices(services =>
            {
                services.AddHttpClient();

                services.AddSingleton<IHttpMessageHandlerBuilderFilter, HttpRequestInterceptionFilter>(
                    _ => new HttpRequestInterceptionFilter(Interceptor));

                services.AddSingleton<IPostConfigureOptions<GitHubAuthenticationOptions>, RemoteAuthorizationEventsFilter>();
                services.AddScoped<LoopbackOAuthEvents>();
            });

            // Configure a bundle of HTTP requests to intercept for the OAuth flow.
            Interceptor.RegisterBundle("oauth-http-bundle.json");
        }
    }