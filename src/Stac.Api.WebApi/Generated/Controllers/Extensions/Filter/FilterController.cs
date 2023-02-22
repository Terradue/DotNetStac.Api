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

#pragma warning disable 108 // Disable "CS0108 '{derivedDto}.ToJson()' hides inherited member '{dtoBase}.ToJson()'. Use the new keyword if hiding was intended."
#pragma warning disable 114 // Disable "CS0114 '{derivedDto}.RaisePropertyChanged(String)' hides inherited member 'dtoBase.RaisePropertyChanged(String)'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword."
#pragma warning disable 472 // Disable "CS0472 The result of the expression is always 'false' since a value of type 'Int32' is never equal to 'null' of type 'Int32?'
#pragma warning disable 1573 // Disable "CS1573 Parameter '...' has no matching param tag in the XML comment for ...
#pragma warning disable 1591 // Disable "CS1591 Missing XML comment for publicly visible type or member ..."
#pragma warning disable 8073 // Disable "CS8073 The result of the expression is always 'false' since a value of type 'T' is never equal to 'null' of type 'T?'"
#pragma warning disable 3016 // Disable "CS3016 Arrays as attribute arguments is not CLS-compliant"
#pragma warning disable 8603 // Disable "CS8603 Possible null reference return"

namespace Stac.Api.WebApi.Controllers.Extensions.Filter
{
    using System = global::System;

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public interface IFilterController
    {

        /// <summary>
        /// Get the JSON Schema defining the list of variable terms that can be used in CQL2 expressions.
        /// </summary>

        /// <returns>A JSON Schema defining the Queryables allowed in CQL2 expressions</returns>

        System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<NJsonSchema.JsonSchema>> GetQueryablesAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Get the JSON Schema defining the list of variable terms that can be used in CQL2 expressions.
        /// </summary>

        /// <param name="collectionId">ID of Collection</param>

        /// <returns>A JSON Schema defining the Queryables allowed in CQL2 expressions</returns>

        System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<NJsonSchema.JsonSchema>> GetQueryablesForCollectionAsync(string collectionId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Search STAC items with simple filtering.
        /// </summary>

        /// <param name="filter">**Extension:** Filter
        /// <br/>
        /// <br/>A CQL2 filter expression for filtering items.</param>

        /// <param name="filter_lang">**Extension:** Filter
        /// <br/>
        /// <br/>The CQL2 filter encoding that the 'filter' value uses. Must be one of 'cql2-text' or 'cql2-json'.</param>

        /// <param name="filter_crs">**Extension:** Filter
        /// <br/>
        /// <br/>The CRS used by spatial predicates in the filter parameter. In STAC API, only value that must be accepted
        /// <br/>is 'http://www.opengis.net/def/crs/OGC/1.3/CRS84'.</param>

        /// <returns>A feature collection.</returns>

        System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<StacFeatureCollection>> GetItemSearchAsync(Models.Cql2.CQL2Filter filter, FilterLang? filter_lang, System.Uri filter_crs, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Search STAC items with full-featured filtering.
        /// </summary>


        /// <returns>A feature collection.</returns>

        System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<StacFeatureCollection>> PostItemSearchAsync(FilterSearchBody body, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

    }

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]

    public partial class FilterController : Stac.Api.WebApi.StacApiController
    {
        private IFilterController _implementation;

        public FilterController(IFilterController implementation)
        {
            _implementation = implementation;
        }

        /// <summary>
        /// Get the JSON Schema defining the list of variable terms that can be used in CQL2 expressions.
        /// </summary>
        /// <returns>A JSON Schema defining the Queryables allowed in CQL2 expressions</returns>
        [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.Route("queryables")]
        public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<NJsonSchema.JsonSchema>> GetQueryables(System.Threading.CancellationToken cancellationToken)
        {

            return _implementation.GetQueryablesAsync(cancellationToken);
        }

        /// <summary>
        /// Get the JSON Schema defining the list of variable terms that can be used in CQL2 expressions.
        /// </summary>
        /// <param name="collectionId">ID of Collection</param>
        /// <returns>A JSON Schema defining the Queryables allowed in CQL2 expressions</returns>
        [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.Route("collections/{collectionId}/queryables")]
        public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<NJsonSchema.JsonSchema>> GetQueryablesForCollection([Microsoft.AspNetCore.Mvc.ModelBinding.BindRequired] string collectionId, System.Threading.CancellationToken cancellationToken)
        {

            return _implementation.GetQueryablesForCollectionAsync(collectionId, cancellationToken);
        }

        /// <summary>
        /// Search STAC items with simple filtering.
        /// </summary>
        /// <param name="filter">**Extension:** Filter
        /// <br/>
        /// <br/>A CQL2 filter expression for filtering items.</param>
        /// <param name="filter_lang">**Extension:** Filter
        /// <br/>
        /// <br/>The CQL2 filter encoding that the 'filter' value uses. Must be one of 'cql2-text' or 'cql2-json'.</param>
        /// <param name="filter_crs">**Extension:** Filter
        /// <br/>
        /// <br/>The CRS used by spatial predicates in the filter parameter. In STAC API, only value that must be accepted
        /// <br/>is 'http://www.opengis.net/def/crs/OGC/1.3/CRS84'.</param>
        /// <returns>A feature collection.</returns>
        [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.Route("search")]
        public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<StacFeatureCollection>> GetItemSearch([Microsoft.AspNetCore.Mvc.FromQuery] [Microsoft.AspNetCore.Mvc.ModelBinding.BindRequired] Models.Cql2.CQL2Filter filter, [Microsoft.AspNetCore.Mvc.FromQuery(Name = "filter-lang")] FilterLang? filter_lang, [Microsoft.AspNetCore.Mvc.FromQuery(Name = "filter-crs")] System.Uri filter_crs, System.Threading.CancellationToken cancellationToken)
        {

            return _implementation.GetItemSearchAsync(filter, filter_lang, filter_crs, cancellationToken);
        }

        /// <summary>
        /// Search STAC items with full-featured filtering.
        /// </summary>
        /// <returns>A feature collection.</returns>
        [Microsoft.AspNetCore.Mvc.HttpPost, Microsoft.AspNetCore.Mvc.Route("search")]
        public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<StacFeatureCollection>> PostItemSearch([Microsoft.AspNetCore.Mvc.FromBody] FilterSearchBody body, System.Threading.CancellationToken cancellationToken)
        {

            return _implementation.PostItemSearchAsync(body, cancellationToken);
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