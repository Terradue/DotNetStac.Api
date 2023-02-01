//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

using Stac;
using Stac.Common;
using Stac.Api.Models;
using Stac.Api.Interfaces;
using Stac.Api.Clients.Collections;
using Stac.Api.Clients.Core;
using Stac.Api.Clients.Extensions.Filter;
using Stac.Api.Clients.Extensions.Query;
using Stac.Api.Clients.Extensions;
using Stac.Api.Clients.Features;

#pragma warning disable 108 // Disable "CS0108 '{derivedDto}.ToJson()' hides inherited member '{dtoBase}.ToJson()'. Use the new keyword if hiding was intended."
#pragma warning disable 114 // Disable "CS0114 '{derivedDto}.RaisePropertyChanged(String)' hides inherited member 'dtoBase.RaisePropertyChanged(String)'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword."
#pragma warning disable 472 // Disable "CS0472 The result of the expression is always 'false' since a value of type 'Int32' is never equal to 'null' of type 'Int32?'
#pragma warning disable 1573 // Disable "CS1573 Parameter '...' has no matching param tag in the XML comment for ...
#pragma warning disable 1591 // Disable "CS1591 Missing XML comment for publicly visible type or member ..."
#pragma warning disable 8073 // Disable "CS8073 The result of the expression is always 'false' since a value of type 'T' is never equal to 'null' of type 'T?'"
#pragma warning disable 3016 // Disable "CS3016 Arrays as attribute arguments is not CLS-compliant"
#pragma warning disable 8603 // Disable "CS8603 Possible null reference return"

namespace Stac.Api.WebApi.Controllers.Features
{
    using System = global::System;

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public interface IFeaturesController
    {

        /// <summary>
        /// information about specifications that this API conforms to
        /// </summary>

        /// <returns>The URIs of all conformance classes supported by the server.
        /// <br/>
        /// <br/>To support "generic" clients that want to access multiple
        /// <br/>OGC API Features implementations - and not "just" a specific
        /// <br/>API / server, the server declares the conformance
        /// <br/>classes it implements and conforms to.</returns>

        System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<ConformanceDeclaration>> GetConformanceDeclarationAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// fetch features
        /// </summary>

        /// <param name="collectionId">local identifier of a collection</param>

        /// <param name="limit">The optional limit parameter recommends the number of items that should be present in the response document.
        /// <br/>
        /// <br/>If the limit parameter value is greater than advertised limit maximum, the server must return the
        /// <br/>maximum possible number of items, rather than responding with an error.
        /// <br/>
        /// <br/>Only items are counted that are on the first level of the collection in the response document.
        /// <br/>Nested objects contained within the explicitly requested items must not be counted.
        /// <br/>
        /// <br/>Minimum = 1. Maximum = 10000. Default = 10.</param>

        /// <param name="bbox">Only features that have a geometry that intersects the bounding box are selected.
        /// <br/>The bounding box is provided as four or six numbers, depending on whether the
        /// <br/>coordinate reference system includes a vertical axis (height or depth):
        /// <br/>
        /// <br/>* Lower left corner, coordinate axis 1
        /// <br/>* Lower left corner, coordinate axis 2
        /// <br/>* Minimum value, coordinate axis 3 (optional)
        /// <br/>* Upper right corner, coordinate axis 1
        /// <br/>* Upper right corner, coordinate axis 2
        /// <br/>* Maximum value, coordinate axis 3 (optional)
        /// <br/>
        /// <br/>The coordinate reference system of the values is WGS 84 longitude/latitude
        /// <br/>(http://www.opengis.net/def/crs/OGC/1.3/CRS84).
        /// <br/>
        /// <br/>For WGS 84 longitude/latitude the values are in most cases the sequence of
        /// <br/>minimum longitude, minimum latitude, maximum longitude and maximum latitude.
        /// <br/>However, in cases where the box spans the antimeridian the first value
        /// <br/>(west-most box edge) is larger than the third value (east-most box edge).
        /// <br/>
        /// <br/>If the vertical axis is included, the third and the sixth number are
        /// <br/>the bottom and the top of the 3-dimensional bounding box.
        /// <br/>
        /// <br/>If a feature has multiple spatial geometry properties, it is the decision of the
        /// <br/>server whether only a single spatial geometry property is used to determine
        /// <br/>the extent or all relevant geometries.</param>

        /// <param name="datetime">Either a date-time or an interval, open or closed. Date and time expressions
        /// <br/>adhere to RFC 3339. Open intervals are expressed using double-dots.
        /// <br/>
        /// <br/>Examples:
        /// <br/>
        /// <br/>* A date-time: "2018-02-12T23:20:50Z"
        /// <br/>* A closed interval: "2018-02-12T00:00:00Z/2018-03-18T12:31:12Z"
        /// <br/>* Open intervals: "2018-02-12T00:00:00Z/.." or "../2018-03-18T12:31:12Z"
        /// <br/>
        /// <br/>Only features that have a temporal property that intersects the value of
        /// <br/>`datetime` are selected.
        /// <br/>
        /// <br/>If a feature has multiple temporal properties, it is the decision of the
        /// <br/>server whether only a single temporal property is used to determine
        /// <br/>the extent or all relevant temporal properties.</param>

        /// <returns>The response is a document consisting of features in the collection.
        /// <br/>The features included in the response are determined by the server
        /// <br/>based on the query parameters of the request. To support access to
        /// <br/>larger collections without overloading the client, the API supports
        /// <br/>paged access with links to the next page, if more features are selected
        /// <br/>that the page size.
        /// <br/>
        /// <br/>The `bbox` and `datetime` parameter can be used to select only a
        /// <br/>subset of the features in the collection (the features that are in the
        /// <br/>bounding box or time interval). The `bbox` parameter matches all features
        /// <br/>in the collection that are not associated with a location, too. The
        /// <br/>`datetime` parameter matches all features in the collection that are
        /// <br/>not associated with a time stamp or interval, too.
        /// <br/>
        /// <br/>The `limit` parameter may be used to control the subset of the
        /// <br/>selected features that should be returned in the response, the page size.
        /// <br/>Each page may include information about the number of selected and
        /// <br/>returned features (`numberMatched` and `numberReturned`) as well as
        /// <br/>links to support paging (link relation `next`).</returns>

        System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<StacFeatureCollection>> GetFeaturesAsync(string collectionId, int limit, string bbox, string datetime, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// fetch a single feature
        /// </summary>

        /// <param name="collectionId">local identifier of a collection</param>

        /// <param name="featureId">local identifier of a feature</param>

        /// <returns>fetch the feature with id `featureId` in the feature collection
        /// <br/>with id `collectionId`</returns>

        System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<StacItem>> GetFeatureAsync(string collectionId, string featureId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

    }

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]

    public partial class FeaturesController : Stac.Api.WebApi.StacApiController
    {
        private IFeaturesController _implementation;

        public FeaturesController(IFeaturesController implementation)
        {
            _implementation = implementation;
        }

        /// <summary>
        /// information about specifications that this API conforms to
        /// </summary>
        /// <returns>The URIs of all conformance classes supported by the server.
        /// <br/>
        /// <br/>To support "generic" clients that want to access multiple
        /// <br/>OGC API Features implementations - and not "just" a specific
        /// <br/>API / server, the server declares the conformance
        /// <br/>classes it implements and conforms to.</returns>
        [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.Route("conformance")]
        public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<ConformanceDeclaration>> GetConformanceDeclaration(System.Threading.CancellationToken cancellationToken)
        {

            return _implementation.GetConformanceDeclarationAsync(cancellationToken);
        }

        /// <summary>
        /// fetch features
        /// </summary>
        /// <param name="collectionId">local identifier of a collection</param>
        /// <param name="limit">The optional limit parameter recommends the number of items that should be present in the response document.
        /// <br/>
        /// <br/>If the limit parameter value is greater than advertised limit maximum, the server must return the
        /// <br/>maximum possible number of items, rather than responding with an error.
        /// <br/>
        /// <br/>Only items are counted that are on the first level of the collection in the response document.
        /// <br/>Nested objects contained within the explicitly requested items must not be counted.
        /// <br/>
        /// <br/>Minimum = 1. Maximum = 10000. Default = 10.</param>
        /// <param name="bbox">Only features that have a geometry that intersects the bounding box are selected.
        /// <br/>The bounding box is provided as four or six numbers, depending on whether the
        /// <br/>coordinate reference system includes a vertical axis (height or depth):
        /// <br/>
        /// <br/>* Lower left corner, coordinate axis 1
        /// <br/>* Lower left corner, coordinate axis 2
        /// <br/>* Minimum value, coordinate axis 3 (optional)
        /// <br/>* Upper right corner, coordinate axis 1
        /// <br/>* Upper right corner, coordinate axis 2
        /// <br/>* Maximum value, coordinate axis 3 (optional)
        /// <br/>
        /// <br/>The coordinate reference system of the values is WGS 84 longitude/latitude
        /// <br/>(http://www.opengis.net/def/crs/OGC/1.3/CRS84).
        /// <br/>
        /// <br/>For WGS 84 longitude/latitude the values are in most cases the sequence of
        /// <br/>minimum longitude, minimum latitude, maximum longitude and maximum latitude.
        /// <br/>However, in cases where the box spans the antimeridian the first value
        /// <br/>(west-most box edge) is larger than the third value (east-most box edge).
        /// <br/>
        /// <br/>If the vertical axis is included, the third and the sixth number are
        /// <br/>the bottom and the top of the 3-dimensional bounding box.
        /// <br/>
        /// <br/>If a feature has multiple spatial geometry properties, it is the decision of the
        /// <br/>server whether only a single spatial geometry property is used to determine
        /// <br/>the extent or all relevant geometries.</param>
        /// <param name="datetime">Either a date-time or an interval, open or closed. Date and time expressions
        /// <br/>adhere to RFC 3339. Open intervals are expressed using double-dots.
        /// <br/>
        /// <br/>Examples:
        /// <br/>
        /// <br/>* A date-time: "2018-02-12T23:20:50Z"
        /// <br/>* A closed interval: "2018-02-12T00:00:00Z/2018-03-18T12:31:12Z"
        /// <br/>* Open intervals: "2018-02-12T00:00:00Z/.." or "../2018-03-18T12:31:12Z"
        /// <br/>
        /// <br/>Only features that have a temporal property that intersects the value of
        /// <br/>`datetime` are selected.
        /// <br/>
        /// <br/>If a feature has multiple temporal properties, it is the decision of the
        /// <br/>server whether only a single temporal property is used to determine
        /// <br/>the extent or all relevant temporal properties.</param>
        /// <returns>The response is a document consisting of features in the collection.
        /// <br/>The features included in the response are determined by the server
        /// <br/>based on the query parameters of the request. To support access to
        /// <br/>larger collections without overloading the client, the API supports
        /// <br/>paged access with links to the next page, if more features are selected
        /// <br/>that the page size.
        /// <br/>
        /// <br/>The `bbox` and `datetime` parameter can be used to select only a
        /// <br/>subset of the features in the collection (the features that are in the
        /// <br/>bounding box or time interval). The `bbox` parameter matches all features
        /// <br/>in the collection that are not associated with a location, too. The
        /// <br/>`datetime` parameter matches all features in the collection that are
        /// <br/>not associated with a time stamp or interval, too.
        /// <br/>
        /// <br/>The `limit` parameter may be used to control the subset of the
        /// <br/>selected features that should be returned in the response, the page size.
        /// <br/>Each page may include information about the number of selected and
        /// <br/>returned features (`numberMatched` and `numberReturned`) as well as
        /// <br/>links to support paging (link relation `next`).</returns>
        [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.Route("collections/{collectionId}/items")]
        public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<StacFeatureCollection>> GetFeatures([Microsoft.AspNetCore.Mvc.ModelBinding.BindRequired] string collectionId, [Microsoft.AspNetCore.Mvc.FromQuery] int? limit, [Microsoft.AspNetCore.Mvc.FromQuery] string bbox, [Microsoft.AspNetCore.Mvc.FromQuery] string datetime, System.Threading.CancellationToken cancellationToken)
        {

            return _implementation.GetFeaturesAsync(collectionId, limit ?? 10, bbox, datetime, cancellationToken);
        }

        /// <summary>
        /// fetch a single feature
        /// </summary>
        /// <param name="collectionId">local identifier of a collection</param>
        /// <param name="featureId">local identifier of a feature</param>
        /// <returns>fetch the feature with id `featureId` in the feature collection
        /// <br/>with id `collectionId`</returns>
        [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.Route("collections/{collectionId}/items/{featureId}")]
        public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<StacItem>> GetFeature([Microsoft.AspNetCore.Mvc.ModelBinding.BindRequired] string collectionId, [Microsoft.AspNetCore.Mvc.ModelBinding.BindRequired] string featureId, System.Threading.CancellationToken cancellationToken)
        {

            return _implementation.GetFeatureAsync(collectionId, featureId, cancellationToken);
        }

    }

    


}

#pragma warning restore 1591
#pragma warning restore 1573
#pragma warning restore  472
#pragma warning restore  114
#pragma warning restore  108
#pragma warning restore 3016
#pragma warning restore 8603