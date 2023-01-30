using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Stac.Api.Clients.Collections;
using Stac.Api.Interfaces;
using Stac.Api.Models;
using Stac.Api.Models.Core;
using Stac.Api.WebApi.Services;

namespace Stac.Api.WebApi.Patterns.CollectionBased
{
    public class CollectionBasedStacLinker : IStacLinker
    {

        #region Implementation of IStacLinker

        public void Link(LandingPage landingPage, IStacApiContext stacApiContext)
        {
            landingPage.Links.Add(StacLink.CreateSelfLink(new Uri(stacApiContext.LinkGenerator.GetUriByAction(stacApiContext.HttpContext, "GetLandingPage", "Core")),
                                     "application/json"));
            landingPage.Links.Add(StacLink.CreateRootLink(new Uri(stacApiContext.LinkGenerator.GetUriByAction(stacApiContext.HttpContext, "GetLandingPage", "Core")),
                                     "application/json"));

        }

        public void Link(StacCollection collection, IStacApiContext stacApiContext)
        {
            collection.Links.Add(GetSelfLink(collection, stacApiContext));
            collection.Links.Add(GetRootLink(stacApiContext));
        }

        public void Link(StacCollections collections, IStacApiContext stacApiContext)
        {
            collections.Links.Add(GetSelfLink(collections, stacApiContext));
            collections.Links.Add(GetRootLink(stacApiContext));
            foreach (var collection in collections.Collections)
            {
                Link(collection, stacApiContext);
            }
        }

        public void Link(StacItem item, IStacApiContext stacApiContext)
        {
            item.Links.Add(GetSelfLink(item, stacApiContext));
            item.Links.Add(GetRootLink(stacApiContext));
        }

        public void Link(StacFeatureCollection collection, IStacApiContext stacApiContext)
        {
            collection.Links.Add(GetSelfLink(collection, stacApiContext));
            collection.Links.Add(GetRootLink(stacApiContext));
            collection.Links.Add(GetParentLink(collection, stacApiContext));
            AddAdditionalLinks(collection, stacApiContext);
        }

        #endregion

        private StacApiLink GetRootLink(IStacApiContext stacApiContext)
        {
            return new StacApiLink(
                GetUriByAction(stacApiContext, "GetLandingPage", "Core", new { }, null),
                "root",
                null,
                "application/json");
        }

        protected StacApiLink GetSelfLink(StacCollections stacCollections, IStacApiContext stacApiContext)
        {
            return new StacApiLink(
                GetUriByAction(stacApiContext, "GetCollections", "Collections", new { }, null),
                "self",
                "Collections",
                "application/json");
        }

        protected StacApiLink GetSelfLink(StacItem stacItem, IStacApiContext stacApiContext)
        {
            return new StacApiLink(
                GetUriByAction(stacApiContext, "GetFeature", "Features", new { collectionId = stacItem.Collection, featureId = stacItem.Id }, null),
                "self",
                stacItem.Title,
                stacItem.MediaType.ToString());
        }

        protected StacApiLink GetSelfLink(StacCollection collection, IStacApiContext stacApiContext)
        {
            return new StacApiLink(
                GetUriByAction(stacApiContext, "GetCollections", "Collections", new { collectionId = collection.Id }, null),
                "self",
                collection.Title,
                collection.MediaType.ToString());
        }

        protected StacApiLink GetSelfLink(StacFeatureCollection collection, IStacApiContext stacApiContext)
        {
            return new StacApiLink(
                new Uri(stacApiContext.LinkGenerator.GetUriByRouteValues(stacApiContext.HttpContext, null, stacApiContext.HttpContext.Request.Query.ToDictionary(x => x.Key, x => x.Value.ToString()))),
                // GetUriByAction(stacApiContext, "GetFeatures", "Features", new { collectionId = collection.Collection }, null),
                "self",
                null,
                "application/geo+json");
        }

        private StacLink GetParentLink(StacFeatureCollection collection, IStacApiContext stacApiContext)
        {
            if (stacApiContext.Collections?.Count() == 1)
            {
                return new StacApiLink(
                                GetUriByAction(stacApiContext, "DescribeCollection", "Collections", new { collectionId = stacApiContext.Collections.First() }, null),
                                "parent",
                                null,
                                "application/json");
            }
            else
            {
                return new StacApiLink(
                    GetUriByAction(stacApiContext, "GetCollections", "Collections", new { }, null),
                    "parent",
                    null,
                    "application/json");
            }
        }

        private Uri GetUriByAction(IStacApiContext stacApiContext, string actionName, string controllerName, object? value, IDictionary<string, object>? queryValues)
        {
            var url = stacApiContext.LinkGenerator.GetUriByAction(stacApiContext.HttpContext, actionName, controllerName, value);
            if (url == null)
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

        internal static void AddAdditionalLinks(ILinksCollectionObject linksCollectionObject, IStacApiContext stacApiContext)
        {
            foreach (ILinkValues linkValue in stacApiContext.LinkValues)
            {
                StacApiLink stacApiLink = CreateStacApiLink(stacApiContext, linkValue);
                linksCollectionObject.Links.Add(stacApiLink);
            }
        }

        private static StacApiLink CreateStacApiLink(IStacApiContext stacApiContext, ILinkValues linkValue)
        {
            string BaseUri = stacApiContext.LinkGenerator.GetUriByAction(stacApiContext.HttpContext, linkValue.ActionName, linkValue.ControllerName);
            UriBuilder uriBuilder = new UriBuilder(BaseUri);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            foreach (var queryValue in linkValue.QueryValues ?? new Dictionary<string, object>())
            {
                query[queryValue.Key] = queryValue.Value.ToString();
            }
            uriBuilder.Query = query.ToString();
            Uri uri = new Uri(uriBuilder.ToString());
            StacApiLink stacApiLink = new StacApiLink(
                uri,
                linkValue.RelationshipType.GetEnumMemberValue(),
                linkValue.Title,
                linkValue.MediaType);
            return stacApiLink;
        }
    }
}