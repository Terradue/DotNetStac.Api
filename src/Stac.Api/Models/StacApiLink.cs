using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stac.Api.Models
{
    public partial class StacApiLink : StacLink
    {
        /// <summary>
        /// Specifies the HTTP method that the resource expects
        /// </summary>
        [Newtonsoft.Json.JsonProperty("method", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public StacApiLinkMethod Method { get; set; } = StacApiLinkMethod.GET;

        /// <summary>
        /// Object key values pairs they map to headers
        /// </summary>
        [Newtonsoft.Json.JsonProperty("headers", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public object Headers { get; set; }

        /// <summary>
        /// For POST requests, the resource can specify the HTTP body as a JSON object.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("body", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public object Body { get; set; }

        /// <summary>
        /// This is only valid when the server is responding to POST request.
        /// <br/>
        /// <br/>If merge is true, the client is expected to merge the body value
        /// <br/>into the current request body before following the link.
        /// <br/>This avoids passing large post bodies back and forth when following
        /// <br/>links, particularly for navigating pages through the `POST /search`
        /// <br/>endpoint.
        /// <br/>
        /// <br/>NOTE: To support form encoding it is expected that a client be able
        /// <br/>to merge in the key value pairs specified as JSON
        /// <br/>`{"next": "token"}` will become `&amp;next=token`.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("merge", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool Merge { get; set; } = false;

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public enum StacApiLinkMethod
    {

        [System.Runtime.Serialization.EnumMember(Value = @"GET")]
        GET = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"POST")]
        POST = 1,

    }


}