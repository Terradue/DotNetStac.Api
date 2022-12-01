using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Web;
using Stac.Api.Clients.Collections;
using Stac.Api.Models;
using Stac.Api.WebApi.Services;

namespace Stac.Api.WebApi.Patterns.CollectionBased
{
    public class CollectionBasedStacLinker : IStacLinker
    {
        private readonly LinkGenerator _linkGenerator;

        public CollectionBasedStacLinker(LinkGenerator linkGenerator)
        {
            _linkGenerator = linkGenerator;
        }

        #region Implementation of IStacLinker

        public void Link(StacCollection collection, HttpContext httpContext)
        {
            collection.Links.Add(GetSelfLink(collection, httpContext));
            collection.Links.Add(GetRootLink(httpContext));
        }

        public void Link(StacCollections collections, HttpContext httpContext)
        {
            collections.Links.Add(GetSelfLink(collections, httpContext));
            collections.Links.Add(GetRootLink(httpContext));
        }

        public void Link<T>(T linksCollectionObject, IStacLinkValuesProvider<T> stacLinkValuesProvider, HttpContext httpContext) where T : ILinksCollectionObject
        {
            GetActionId<T>(out string actionId, out string controllerId);
            foreach (var linkValue in stacLinkValuesProvider.GetLinkValues())
            {
                GetUriByAction(httpContext, actionId, controllerId, linkValue.RouteValues, linkValue.QueryValues);
            }
        }

        #endregion

        private StacLink GetRootLink(HttpContext httpContext)
        {
            return new StacApiLink(
                GetUriByAction(httpContext, "GetLandingPage", "Core", new { }),
                "root",
                null,
                "application/json");
        }

        protected StacApiLink GetSelfLink(StacCollections stacCollections, HttpContext httpContext)
        {
            return new StacApiLink(
                GetUriByAction(httpContext, "GetCollections", "Collections", new { }, null),
                "self",
                "Collections",
                "application/json");
        }

        protected StacApiLink GetSelfLink(StacCollection collection, HttpContext httpContext)
        {
            return new StacApiLink(
                GetUriByAction(httpContext, "GetCollections", "Collections", new { collectionId = collection.Id }, null),
                "self",
                collection.Title,
                "application/json");
        }

        private Uri GetUriByAction(HttpContext httpContext, string actionName, string controllerName, object? value, IDictionary<string, object>? queryValues)
        {
            var url = _linkGenerator.GetUriByAction(httpContext, actionName, controllerName, value);
            if ( url == null )
            {
                throw new InvalidOperationException($"Could not generate URL for action {actionName} on controller {controllerName} with value {value}");
            }
            UriBuilder uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            foreach (var queryValue in queryValues ?? new Dictionary<string, object>())
            {
                query[queryValue.Key] = queryValue.Value.ToString();
            }
            uriBuilder.Query = query.ToString();
            return new Uri(uriBuilder.ToString());
        }

        private void GetActionId<T>(out string actionId, out string controllerId) where T : ILinksCollectionObject
        {
            throw new NotImplementedException();
        }
    }
}