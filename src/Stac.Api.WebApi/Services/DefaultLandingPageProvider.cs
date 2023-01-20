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
        private readonly IStacApiContextFactory _stacApiContextFactory;
        private readonly IStacLinker _stacLinker;

        public LinkGenerator LinkGenerator { get; }

        public DefaultLandingPageProvider(IStacApiEndpointManager stacApiEndpointManager,
                                          IDataServicesProvider dataServicesProvider,
                                          LinkGenerator linkGenerator,
                                          IStacApiContextFactory stacApiContextFactory,
                                          IStacLinker stacLinker)
        {
            _stacApiEndpointManager = stacApiEndpointManager;
            _dataServicesProvider = dataServicesProvider;
            LinkGenerator = linkGenerator;
            _stacApiContextFactory = stacApiContextFactory;
            _stacLinker = stacLinker;
        }

        public async Task<LandingPage> GetLandingPageAsync(CancellationToken cancellationToken)
        {
            // Create a new StacApiContext
            var stacApiContext = _stacApiContextFactory.Create();

            var rootCatalogProvider = _dataServicesProvider.GetRootCatalogProvider();

            var landingPage = new LandingPage(await rootCatalogProvider.GetRootCatalogAsync(stacApiContext, cancellationToken));

            AddConformanceClasses(landingPage);

            AddLinks(landingPage, stacApiContext);

            return landingPage;
        }

        private LandingPage AddLinks(LandingPage landingPage, IStacApiContext stacApiContext)
        {
            _stacLinker.Link(landingPage, stacApiContext);
            AddEnpointLinks(landingPage, stacApiContext);
            return landingPage;
        }

        private LandingPage AddEnpointLinks(LandingPage landingPage, IStacApiContext stacApiContext)
        {
            var stacapiControllers = _stacApiEndpointManager.GetRegisteredStacApiControllers();
            foreach (var controller in stacapiControllers)
            {
                var links = controller.GetLandingPageLinks(stacApiContext);
                landingPage.Links.AddRange(links);
            }
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