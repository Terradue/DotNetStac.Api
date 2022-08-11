//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

using Stac;
using Stac.Api.Models;

#pragma warning disable 108 // Disable "CS0108 '{derivedDto}.ToJson()' hides inherited member '{dtoBase}.ToJson()'. Use the new keyword if hiding was intended."
#pragma warning disable 114 // Disable "CS0114 '{derivedDto}.RaisePropertyChanged(String)' hides inherited member 'dtoBase.RaisePropertyChanged(String)'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword."
#pragma warning disable 472 // Disable "CS0472 The result of the expression is always 'false' since a value of type 'Int32' is never equal to 'null' of type 'Int32?'
#pragma warning disable 1573 // Disable "CS1573 Parameter '...' has no matching param tag in the XML comment for ...
#pragma warning disable 1591 // Disable "CS1591 Missing XML comment for publicly visible type or member ..."
#pragma warning disable 8073 // Disable "CS8073 The result of the expression is always 'false' since a value of type 'T' is never equal to 'null' of type 'T?'"
#pragma warning disable 3016 // Disable "CS3016 Arrays as attribute arguments is not CLS-compliant"
#pragma warning disable 8603 // Disable "CS8603 Possible null reference return"

namespace Stac.Api.WebApi.Controllers.ItemSearch
{
    using System = global::System;

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public interface IItemSearchController
    {

        /// <summary>
        /// Search STAC items with simple filtering.
        /// </summary>

        /// <param name="bbox">Only features that have a geometry that intersects the bounding box are selected.
        /// <br/>The bounding box is provided as four or six numbers, depending on
        /// <br/>whether the coordinate reference system includes a vertical axis (height
        /// <br/>or depth):
        /// <br/>
        /// <br/>* Lower left corner, coordinate axis 1
        /// <br/>* Lower left corner, coordinate axis 2
        /// <br/>* Minimum value, coordinate axis 3 (optional)
        /// <br/>* Upper right corner, coordinate axis 1
        /// <br/>* Upper right corner, coordinate axis 2
        /// <br/>* Maximum value, coordinate axis 3 (optional)
        /// <br/>
        /// <br/>The coordinate reference system of the values is WGS 84
        /// <br/>longitude/latitude (http://www.opengis.net/def/crs/OGC/1.3/CRS84).
        /// <br/>
        /// <br/>For WGS 84 longitude/latitude the values are in most cases the sequence
        /// <br/>of minimum longitude, minimum latitude, maximum longitude and maximum
        /// <br/>latitude. However, in cases where the box spans the antimeridian the
        /// <br/>first value (west-most box edge) is larger than the third value
        /// <br/>(east-most box edge).
        /// <br/>
        /// <br/>If the vertical axis is included, the third and the sixth number are
        /// <br/>the bottom and the top of the 3-dimensional bounding box.
        /// <br/>
        /// <br/>If a feature has multiple spatial geometry properties, it is the
        /// <br/>decision of the server whether only a single spatial geometry property
        /// <br/>is used to determine the extent or all relevant geometries.
        /// <br/>
        /// <br/>Example: The bounding box of the New Zealand Exclusive Economic Zone in
        /// <br/>WGS 84 (from 160.6°E to 170°W and from 55.95°S to 25.89°S) would be
        /// <br/>represented in JSON as `[160.6, -55.95, -170, -25.89]` and in a query as
        /// <br/>`bbox=160.6,-55.95,-170,-25.89`.</param>

        /// <param name="intersectsQueryString">The optional intersects parameter filters the result Items in the same was as bbox, only with
        /// <br/>a GeoJSON Geometry rather than a bbox.</param>

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

        /// <param name="limit">The optional limit parameter recommends the number of items that should be present in the response document.
        /// <br/>
        /// <br/>Only items are counted that are on the first level of the collection in the response document.
        /// <br/>Nested objects contained within the explicitly requested items must not be counted.
        /// <br/>
        /// <br/>Minimum = 1. Maximum = 10000. Default = 10.</param>

        /// <param name="ids">Array of Item ids to return.</param>

        /// <param name="collections">Array of Collection IDs to include in the search for items.
        /// <br/>Only Item objects in one of the provided collections will be searched</param>

        /// <param name="fields">**Extension:** Fields
        /// <br/>
        /// <br/>Determines the shape of the features in the response</param>

        /// <param name="sortby">**Extension:** Sort
        /// <br/>
        /// <br/>An array of property names, prefixed by either "+" for ascending or
        /// <br/>"-" for descending. If no prefix is provided, "+" is assumed.</param>

        /// <param name="filterParameter">**Extension:** Filter
        /// <br/>
        /// <br/>A CQL2 filter expression for filtering items.</param>

        /// <returns>A feature collection.</returns>

        System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<SearchResponse>> GetItemSearchAsync(string bbox, IntersectsQueryString intersectsQueryString, string datetime, int limit, System.Collections.Generic.IEnumerable<string> ids, System.Collections.Generic.IEnumerable<string> collections, string fields, string sortby, Stac.Api.WebApi.Controllers.Fragments.Filter.FilterParameter filterParameter, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Search STAC items with full-featured filtering.
        /// </summary>


        /// <returns>A feature collection.</returns>

        System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<SearchResponse>> PostItemSearchAsync(SearchRequest body, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

    }

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]

    public partial class ItemSearchController : Stac.Api.WebApi.StacApiController
    {
        private IItemSearchController _implementation;

        public ItemSearchController(IItemSearchController implementation)
        {
            _implementation = implementation;
        }

        /// <summary>
        /// Search STAC items with simple filtering.
        /// </summary>
        /// <param name="bbox">Only features that have a geometry that intersects the bounding box are selected.
        /// <br/>The bounding box is provided as four or six numbers, depending on
        /// <br/>whether the coordinate reference system includes a vertical axis (height
        /// <br/>or depth):
        /// <br/>
        /// <br/>* Lower left corner, coordinate axis 1
        /// <br/>* Lower left corner, coordinate axis 2
        /// <br/>* Minimum value, coordinate axis 3 (optional)
        /// <br/>* Upper right corner, coordinate axis 1
        /// <br/>* Upper right corner, coordinate axis 2
        /// <br/>* Maximum value, coordinate axis 3 (optional)
        /// <br/>
        /// <br/>The coordinate reference system of the values is WGS 84
        /// <br/>longitude/latitude (http://www.opengis.net/def/crs/OGC/1.3/CRS84).
        /// <br/>
        /// <br/>For WGS 84 longitude/latitude the values are in most cases the sequence
        /// <br/>of minimum longitude, minimum latitude, maximum longitude and maximum
        /// <br/>latitude. However, in cases where the box spans the antimeridian the
        /// <br/>first value (west-most box edge) is larger than the third value
        /// <br/>(east-most box edge).
        /// <br/>
        /// <br/>If the vertical axis is included, the third and the sixth number are
        /// <br/>the bottom and the top of the 3-dimensional bounding box.
        /// <br/>
        /// <br/>If a feature has multiple spatial geometry properties, it is the
        /// <br/>decision of the server whether only a single spatial geometry property
        /// <br/>is used to determine the extent or all relevant geometries.
        /// <br/>
        /// <br/>Example: The bounding box of the New Zealand Exclusive Economic Zone in
        /// <br/>WGS 84 (from 160.6°E to 170°W and from 55.95°S to 25.89°S) would be
        /// <br/>represented in JSON as `[160.6, -55.95, -170, -25.89]` and in a query as
        /// <br/>`bbox=160.6,-55.95,-170,-25.89`.</param>
        /// <param name="intersectsQueryString">The optional intersects parameter filters the result Items in the same was as bbox, only with
        /// <br/>a GeoJSON Geometry rather than a bbox.</param>
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
        /// <param name="limit">The optional limit parameter recommends the number of items that should be present in the response document.
        /// <br/>
        /// <br/>Only items are counted that are on the first level of the collection in the response document.
        /// <br/>Nested objects contained within the explicitly requested items must not be counted.
        /// <br/>
        /// <br/>Minimum = 1. Maximum = 10000. Default = 10.</param>
        /// <param name="ids">Array of Item ids to return.</param>
        /// <param name="collections">Array of Collection IDs to include in the search for items.
        /// <br/>Only Item objects in one of the provided collections will be searched</param>
        /// <param name="fields">**Extension:** Fields
        /// <br/>
        /// <br/>Determines the shape of the features in the response</param>
        /// <param name="sortby">**Extension:** Sort
        /// <br/>
        /// <br/>An array of property names, prefixed by either "+" for ascending or
        /// <br/>"-" for descending. If no prefix is provided, "+" is assumed.</param>
        /// <param name="filterParameter">**Extension:** Filter
        /// <br/>
        /// <br/>A CQL2 filter expression for filtering items.</param>
        /// <returns>A feature collection.</returns>
        [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.Route("search")]
        public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<SearchResponse>> GetItemSearch([Microsoft.AspNetCore.Mvc.FromQuery] string bbox, [Microsoft.AspNetCore.Mvc.FromQuery] IntersectsQueryString intersectsQueryString, [Microsoft.AspNetCore.Mvc.FromQuery] string datetime, [Microsoft.AspNetCore.Mvc.FromQuery] int? limit, [Microsoft.AspNetCore.Mvc.FromQuery] System.Collections.Generic.IEnumerable<string> ids, [Microsoft.AspNetCore.Mvc.FromQuery] System.Collections.Generic.IEnumerable<string> collections, [Microsoft.AspNetCore.Mvc.FromQuery] string fields, [Microsoft.AspNetCore.Mvc.FromQuery] string sortby, [Microsoft.AspNetCore.Mvc.ModelBinding.BindRequired] Stac.Api.WebApi.Controllers.Fragments.Filter.FilterParameter filterParameter, System.Threading.CancellationToken cancellationToken)
        {

            return _implementation.GetItemSearchAsync(bbox, intersectsQueryString, datetime, limit ?? 10, ids, collections, fields, sortby, filterParameter, cancellationToken);
        }

        /// <summary>
        /// Search STAC items with full-featured filtering.
        /// </summary>
        /// <returns>A feature collection.</returns>
        [Microsoft.AspNetCore.Mvc.HttpPost, Microsoft.AspNetCore.Mvc.Route("search")]
        public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<SearchResponse>> PostItemSearch([Microsoft.AspNetCore.Mvc.FromBody] SearchRequest body, System.Threading.CancellationToken cancellationToken)
        {

            return _implementation.PostItemSearchAsync(body, cancellationToken);
        }

    }

    /// <summary>
    /// The search criteria
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class SearchBody : BboxFilter
    {
        [Newtonsoft.Json.JsonProperty("datetime", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Datetime { get; set; }

        [Newtonsoft.Json.JsonProperty("intersects", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public GeoJSON.Net.Geometry.IGeometryObject Intersects { get; set; }

        [Newtonsoft.Json.JsonProperty("collections", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public CollectionsArray Collections { get; set; }

        [Newtonsoft.Json.JsonProperty("ids", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Ids Ids { get; set; }

        [Newtonsoft.Json.JsonProperty("limit", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.Range(1, 10000)]
        public int Limit { get; set; }

    }

    /// <summary>
    /// Only return items that intersect the provided bounding box.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class BboxFilter
    {
        [Newtonsoft.Json.JsonProperty("bbox", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.MinLength(4)]
        [System.ComponentModel.DataAnnotations.MaxLength(6)]
        public Bbox Bbox { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties = new System.Collections.Generic.Dictionary<string, object>();

        [Newtonsoft.Json.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }

    }

    /// <summary>
    /// Array of Collection IDs to include in the search for items.
    /// <br/>Only Item objects in one of the provided collections will be searched.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class CollectionsArray : System.Collections.ObjectModel.Collection<string>
    {

    }

    /// <summary>
    /// Array of Item ids to return.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class Ids : System.Collections.ObjectModel.Collection<string>
    {

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class SearchRequest : SearchBody
    {
        [Newtonsoft.Json.JsonProperty("fields", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Controllers.Fragments.Fields.Fields Fields { get; set; }

        [Newtonsoft.Json.JsonProperty("filter", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Stac.Api.WebApi.Controllers.Fragments.Filter.FilterParameter Filter { get; set; }

        [Newtonsoft.Json.JsonProperty("filter-lang", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public Controllers.Fragments.Filter.FilterLang FilterLang { get; set; }

        [Newtonsoft.Json.JsonProperty("filter-crs", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Uri FilterCrs { get; set; }

        [Newtonsoft.Json.JsonProperty("sortby", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.MinLength(1)]
        public Controllers.Fragments.Sort.Sortby Sortby { get; set; }

    }

    /// <summary>
    /// Only features that have a geometry that intersects the bounding box are
    /// <br/>selected. The bounding box is provided as four or six numbers,
    /// <br/>depending on whether the coordinate reference system includes a
    /// <br/>vertical axis (elevation or depth):
    /// <br/>
    /// <br/>* Lower left corner, coordinate axis 1
    /// <br/>* Lower left corner, coordinate axis 2  
    /// <br/>* Lower left corner, coordinate axis 3 (optional) 
    /// <br/>* Upper right corner, coordinate axis 1 
    /// <br/>* Upper right corner, coordinate axis 2 
    /// <br/>* Upper right corner, coordinate axis 3 (optional)
    /// <br/>
    /// <br/>The coordinate reference system of the values is WGS84
    /// <br/>longitude/latitude (http://www.opengis.net/def/crs/OGC/1.3/CRS84).
    /// <br/>
    /// <br/>For WGS84 longitude/latitude the values are in most cases the sequence
    /// <br/>of minimum longitude, minimum latitude, maximum longitude and maximum
    /// <br/>latitude. However, in cases where the box spans the antimeridian the
    /// <br/>first value (west-most box edge) is larger than the third value
    /// <br/>(east-most box edge).
    /// <br/>
    /// <br/>If a feature has multiple spatial geometry properties, it is the
    /// <br/>decision of the server whether only a single spatial geometry property
    /// <br/>is used to determine the extent or all relevant geometries.
    /// <br/>
    /// <br/>Example: The bounding box of the New Zealand Exclusive Economic Zone in
    /// <br/>WGS 84 (from 160.6°E to 170°W and from 55.95°S to 25.89°S) would be
    /// <br/>represented in JSON as `[160.6, -55.95, -170, -25.89]` and in a query as
    /// <br/>`bbox=160.6,-55.95,-170,-25.89`.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class Bbox : System.Collections.ObjectModel.Collection<double>
    {

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class Context
    {
        [Newtonsoft.Json.JsonProperty("returned", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Range(0, int.MaxValue)]
        public int Returned { get; set; }

        [Newtonsoft.Json.JsonProperty("limit", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.Range(0, int.MaxValue)]
        public int? Limit { get; set; }

        [Newtonsoft.Json.JsonProperty("matched", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.Range(0, int.MaxValue)]
        public int Matched { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties = new System.Collections.Generic.Dictionary<string, object>();

        [Newtonsoft.Json.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
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