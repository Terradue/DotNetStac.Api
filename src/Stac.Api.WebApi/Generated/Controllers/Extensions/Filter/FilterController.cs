//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

using Stac;
using Stac.Common;
using Stac.Api.Models;

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

    }

    /// <summary>
    /// The CQL2 filter encoding that the 'filter' value uses.
    /// <br/>
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public enum FilterLang
    {

        [System.Runtime.Serialization.EnumMember(Value = @"cql2-text")]
        Cql2Text = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"cql2-json")]
        Cql2Json = 1,

    }

    /// <summary>
    /// Information about the exception: an error code plus an optional description.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class Exception
    {
        [Newtonsoft.Json.JsonProperty("code", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Code { get; set; }

        [Newtonsoft.Json.JsonProperty("description", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Description { get; set; }

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