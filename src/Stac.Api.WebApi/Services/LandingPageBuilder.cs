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

        public LinkGenerator LinkGenerator { get; }
        public IHttpContextAccessor HttpContextAccessor { get; }

        public LandingPageBuilder(IServiceProvider serviceProvider,
                                  LinkGenerator linkGenerator,
                                  IHttpContextAccessor httpContextAccessor)
        {
            _serviceProvider = serviceProvider;
            LinkGenerator = linkGenerator;
            HttpContextAccessor = httpContextAccessor;
        }

        public LandingPage Build(StacCatalog rootCatalog)
        {
            var landingPage = new LandingPage(rootCatalog);

            AddConformanceClasses(landingPage);

            AddLinks(landingPage);

            return landingPage;
        }

        private LandingPage AddLinks(LandingPage landingPage)
        {
            AddBaseLinks(landingPage);
            AddEnpointLinks(landingPage);
            return landingPage;
        }

        private LandingPage AddEnpointLinks(LandingPage landingPage)
        {
            var stacapiControllers = GetAllStacApiControllers();
            foreach (var controller in stacapiControllers)
            {
                var links = controller.GetLandingPageLinks(LinkGenerator, HttpContextAccessor);
                landingPage.Links.AddRange(links);
            }
            return landingPage;
        }

        private LandingPage AddBaseLinks(LandingPage landingPage)
        {
            landingPage.Links.Add(StacLink.CreateSelfLink(new Uri(LinkGenerator.GetUriByAction(HttpContextAccessor.HttpContext, "GetLandingPage", "Core")),
                                     "application/json"));
            landingPage.Links.Add(StacLink.CreateRootLink(new Uri(LinkGenerator.GetUriByAction(HttpContextAccessor.HttpContext, "GetLandingPage", "Core")),
                                     "application/json"));

            return landingPage;
        }

        public LandingPage AddConformanceClasses(LandingPage landingPage)
        {
            var stacapiControllers = GetAllStacApiControllers();
            foreach (var controller in stacapiControllers)
            {
                var conformanceClasses = controller.GetConformanceClasses();
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
                .Distinct()
                .Select(t => (IStacApiController)ActivatorUtilities.CreateInstance(_serviceProvider, t))
                .ToList();
        }
    }
}