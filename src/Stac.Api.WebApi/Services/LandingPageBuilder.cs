using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Controllers;
using Stac.Api.Interfaces;
using Stac.Api.Models;

namespace Stac.Api.WebApi.Services
{
    public class LandingPageBuilder : ILandingPageBuilder
    {
        private readonly IServiceProvider _serviceProvider;

        public LandingPageBuilder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public LandingPage Build(StacCatalog rootCatalog, LinkGenerator linkGenerator)
        {
            var landingPage = new LandingPage(rootCatalog);

            AddConformanceClasses(landingPage);

            // landingPage.Links.Add(StacLink.CreateSelfLink(linkGenerator.GetAppBaseUrl(), "application/json"));
            // landingPage.Links.Add(StacLink.CreateRootLink(linkGenerator.GetAppBaseUrl(), "application/json"));
            // landingPage.Links.Add(new StacLink(new Uri(linkGenerator.GetAppBaseUrl(), "/swagger/v1/swagger.json"), "service-desc", null, "application/vnd.oai.openapi+json;version=3.0"));
            // landingPage.Links.Add(new StacLink(new Uri(linkGenerator.GetAppBaseUrl(), "/swagger"), "service-doc", null, "text/html"));
            // landingPage.Links.Add(new StacLink(new Uri(linkGenerator.GetAppBaseUrl(), "/collections"), "data", null, "application/json"));

            return landingPage;
        }

        public LandingPage AddConformanceClasses(LandingPage landingPage)
        {
            var stacapiControllers = GetAllStacApiControllers();
            foreach (var controller in stacapiControllers)
            {
                var conformanceClasses =controller.GetConformanceClasses();
                landingPage.ConformanceClasses.AddRange(conformanceClasses);
            }
            return landingPage;
        }

        private IEnumerable<IStacApiController> GetAllStacApiControllers()
        {
            var endpointSources = _serviceProvider.GetServices<EndpointDataSource>();
            var endpoints = endpointSources
                .SelectMany(es => es.Endpoints)
                .OfType<RouteEndpoint>();
            var controllers = endpoints
                .Select(e => e.Metadata.GetMetadata<ControllerActionDescriptor>())
                .Where(c => c != null);
            return controllers.Select(c => c.ControllerTypeInfo)
                .Where(t => typeof(IStacApiController).IsAssignableFrom(t))
                .Select(t => (IStacApiController)ActivatorUtilities.CreateInstance(_serviceProvider, t))
                .ToList();
        }
    }
}