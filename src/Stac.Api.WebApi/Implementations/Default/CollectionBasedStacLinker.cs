using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
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

        public void Link(StacItem item, HttpContext httpContext)
        {
            item.Links.Add(GetSelfLink(item, httpContext));
            item.Links.Add(GetRootLink(httpContext));
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

        private StacApiLink GetRootLink(HttpContext httpContext)
        {
            return new StacApiLink(
                GetUriByAction(httpContext, "GetLandingPage", "Core", new { }, null),
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

        protected StacApiLink GetSelfLink(StacItem stacItem, HttpContext httpContext)
        {
            return new StacApiLink(
                GetUriByAction(httpContext, "GetFeature", "Features", new { collectionId = stacItem.Collection, featureId = stacItem.Id }, null),
                "self",
                stacItem.Title,
                stacItem.MediaType.ToString());
        }

        protected StacApiLink GetSelfLink(StacCollection collection, HttpContext httpContext)
        {
            return new StacApiLink(
                GetUriByAction(httpContext, "GetCollections", "Collections", new { collectionId = collection.Id }, null),
                "self",
                collection.Title,
                collection.MediaType.ToString());
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