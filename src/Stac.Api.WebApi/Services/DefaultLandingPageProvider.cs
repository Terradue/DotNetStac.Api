using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Stac.Api.Interfaces;
using Stac.Api.Models;

namespace Stac.Api.WebApi.Services
{
    public class DefaultLandingPageProvider : ILandingPageProvider
    {
        private readonly IStacApiEndpointManager _stacApiEndpointManager;
        private readonly IDataServicesProvider _dataServicesProvider;

        public LinkGenerator LinkGenerator { get; }
        public IHttpContextAccessor HttpContextAccessor { get; }

        public DefaultLandingPageProvider(IStacApiEndpointManager stacApiEndpointManager,
                                          IDataServicesProvider dataServicesProvider,
                                          LinkGenerator linkGenerator,
                                          IHttpContextAccessor httpContextAccessor)
        {
            _stacApiEndpointManager = stacApiEndpointManager;
            _dataServicesProvider = dataServicesProvider;
            LinkGenerator = linkGenerator;
            HttpContextAccessor = httpContextAccessor;
        }

        public async Task<LandingPage> GetLandingPageAsync(CancellationToken cancellationToken)
        {
            var rootCatalogProvider = _dataServicesProvider.GetRootCatalogProvider(HttpContextAccessor.HttpContext);

            var landingPage = new LandingPage(await rootCatalogProvider.GetRootCatalogAsync());

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
            var stacapiControllers = _stacApiEndpointManager.GetRegisteredStacApiControllers();
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
            var conformanceClasses = _stacApiEndpointManager.GetConformanceClasses();
            landingPage.ConformanceClasses.AddRange(conformanceClasses);
            return landingPage;
        }

    }
}