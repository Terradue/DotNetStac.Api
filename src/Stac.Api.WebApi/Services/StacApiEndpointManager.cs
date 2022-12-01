using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Controllers;
using Stac.Api.Interfaces;
using Stac.Api.Models;

namespace Stac.Api.WebApi.Services
{
    public class StacApiEndpointManager : IStacApiEndpointManager
    {
        private readonly IServiceProvider _serviceProvider;


        public StacApiEndpointManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IReadOnlyCollection<string> GetConformanceClasses(bool OgcApiOnly = false)
        {
            var conformanceClasses = new List<string>();
            var stacapiControllers = GetRegisteredStacApiControllers();
            foreach (var controller in stacapiControllers)
            {
                var cc = controller.GetConformanceClasses();
                conformanceClasses.AddRange(cc);
            }
            return conformanceClasses.Distinct().ToList();
        }

        public IEnumerable<IStacApiController> GetRegisteredStacApiControllers()
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