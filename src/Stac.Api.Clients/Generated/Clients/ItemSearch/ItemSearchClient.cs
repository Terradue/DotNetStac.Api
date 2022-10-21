//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

using Stac;
using Stac.Api.Models;
using Stac.Api.WebApi.Controllers.ItemSearch;

#pragma warning disable 108 // Disable "CS0108 '{derivedDto}.ToJson()' hides inherited member '{dtoBase}.ToJson()'. Use the new keyword if hiding was intended."
#pragma warning disable 114 // Disable "CS0114 '{derivedDto}.RaisePropertyChanged(String)' hides inherited member 'dtoBase.RaisePropertyChanged(String)'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword."
#pragma warning disable 472 // Disable "CS0472 The result of the expression is always 'false' since a value of type 'Int32' is never equal to 'null' of type 'Int32?'
#pragma warning disable 1573 // Disable "CS1573 Parameter '...' has no matching param tag in the XML comment for ...
#pragma warning disable 1591 // Disable "CS1591 Missing XML comment for publicly visible type or member ..."
#pragma warning disable 8073 // Disable "CS8073 The result of the expression is always 'false' since a value of type 'T' is never equal to 'null' of type 'T?'"
#pragma warning disable 3016 // Disable "CS3016 Arrays as attribute arguments is not CLS-compliant"
#pragma warning disable 8603 // Disable "CS8603 Possible null reference return"

namespace Stac.Api.Clients.ItemSearch
{
    using System = global::System;

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class ItemSearchClient : Stac.Api.Clients.StacApiClient
    {
        private System.Net.Http.HttpClient _httpClient;
        private System.Lazy<Newtonsoft.Json.JsonSerializerSettings> _settings;

        public ItemSearchClient(string baseUrl, System.Net.Http.HttpClient httpClient)
        {
            BaseUrl = baseUrl;
            _httpClient = httpClient;
            _settings = new System.Lazy<Newtonsoft.Json.JsonSerializerSettings>(CreateSerializerSettings);
        }

        private Newtonsoft.Json.JsonSerializerSettings CreateSerializerSettings()
        {
            var settings = new Newtonsoft.Json.JsonSerializerSettings();
            UpdateJsonSerializerSettings(settings);
            return settings;
        }

        protected Newtonsoft.Json.JsonSerializerSettings JsonSerializerSettings { get { return _settings.Value; } }

        partial void UpdateJsonSerializerSettings(Newtonsoft.Json.JsonSerializerSettings settings);

        partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, string url);
        partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, System.Text.StringBuilder urlBuilder);
        partial void ProcessResponse(System.Net.Http.HttpClient client, System.Net.Http.HttpResponseMessage response);

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
        /// <returns>A feature collection.</returns>
        /// <exception cref="StacApiException">A server side error occurred.</exception>
        public virtual System.Threading.Tasks.Task<StacFeatureCollection> GetItemSearchAsync(string bbox, IntersectsQueryString intersectsQueryString, string datetime, int? limit, System.Collections.Generic.IEnumerable<string> ids, System.Collections.Generic.IEnumerable<string> collections)
        {
            return GetItemSearchAsync(bbox, intersectsQueryString, datetime, limit, ids, collections, System.Threading.CancellationToken.None);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
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
        /// <returns>A feature collection.</returns>
        /// <exception cref="StacApiException">A server side error occurred.</exception>
        public virtual async System.Threading.Tasks.Task<StacFeatureCollection> GetItemSearchAsync(string bbox, IntersectsQueryString intersectsQueryString, string datetime, int? limit, System.Collections.Generic.IEnumerable<string> ids, System.Collections.Generic.IEnumerable<string> collections, System.Threading.CancellationToken cancellationToken)
        {
            var urlBuilder_ = new System.Text.StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/search?");
            if (bbox != null)
            {
                urlBuilder_.Append(System.Uri.EscapeDataString("bbox") + "=").Append(System.Uri.EscapeDataString(ConvertToString(bbox, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            }
            if (intersectsQueryString != null)
            {
                urlBuilder_.Append(System.Uri.EscapeDataString("IntersectsQueryString") + "=").Append(System.Uri.EscapeDataString(ConvertToString(intersectsQueryString, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            }
            if (datetime != null)
            {
                urlBuilder_.Append(System.Uri.EscapeDataString("datetime") + "=").Append(System.Uri.EscapeDataString(ConvertToString(datetime, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            }
            if (limit != null)
            {
                urlBuilder_.Append(System.Uri.EscapeDataString("limit") + "=").Append(System.Uri.EscapeDataString(ConvertToString(limit, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            }
            if (ids != null)
            {
                foreach (var item_ in ids) { urlBuilder_.Append(System.Uri.EscapeDataString("ids") + "=").Append(System.Uri.EscapeDataString(ConvertToString(item_, System.Globalization.CultureInfo.InvariantCulture))).Append("&"); }
            }
            if (collections != null)
            {
                foreach (var item_ in collections) { urlBuilder_.Append(System.Uri.EscapeDataString("collections") + "=").Append(System.Uri.EscapeDataString(ConvertToString(item_, System.Globalization.CultureInfo.InvariantCulture))).Append("&"); }
            }
            urlBuilder_.Length--;

            var client_ = _httpClient;
            var disposeClient_ = false;
            try
            {
                using (var request_ = new System.Net.Http.HttpRequestMessage())
                {
                    request_.Method = new System.Net.Http.HttpMethod("GET");
                    request_.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/geo+json"));

                    PrepareRequest(client_, request_, urlBuilder_);

                    var url_ = urlBuilder_.ToString();
                    request_.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);

                    PrepareRequest(client_, request_, url_);

                    var response_ = await client_.SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse_ = true;
                    try
                    {
                        var headers_ = System.Linq.Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                        if (response_.Content != null && response_.Content.Headers != null)
                        {
                            foreach (var item_ in response_.Content.Headers)
                                headers_[item_.Key] = item_.Value;
                        }

                        ProcessResponse(client_, response_);

                        var status_ = (int)response_.StatusCode;
                        if (status_ == 200)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<StacFeatureCollection>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new StacApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }
                            return objectResponse_.Object;
                        }
                        else
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<StacFeatureCollection>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new StacApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }
                            throw new StacApiException<StacFeatureCollection>("An error occurred.", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                    }
                    finally
                    {
                        if (disposeResponse_)
                            response_.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient_)
                    client_.Dispose();
            }
        }

        /// <summary>
        /// Search STAC items with full-featured filtering.
        /// </summary>
        /// <returns>A feature collection.</returns>
        /// <exception cref="StacApiException">A server side error occurred.</exception>
        public virtual System.Threading.Tasks.Task<StacFeatureCollection> PostItemSearchAsync(SearchBody body)
        {
            return PostItemSearchAsync(body, System.Threading.CancellationToken.None);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>
        /// Search STAC items with full-featured filtering.
        /// </summary>
        /// <returns>A feature collection.</returns>
        /// <exception cref="StacApiException">A server side error occurred.</exception>
        public virtual async System.Threading.Tasks.Task<StacFeatureCollection> PostItemSearchAsync(SearchBody body, System.Threading.CancellationToken cancellationToken)
        {
            var urlBuilder_ = new System.Text.StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/search");

            var client_ = _httpClient;
            var disposeClient_ = false;
            try
            {
                using (var request_ = new System.Net.Http.HttpRequestMessage())
                {
                    var content_ = new System.Net.Http.StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(body, _settings.Value));
                    content_.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json");
                    request_.Content = content_;
                    request_.Method = new System.Net.Http.HttpMethod("POST");
                    request_.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/geo+json"));

                    PrepareRequest(client_, request_, urlBuilder_);

                    var url_ = urlBuilder_.ToString();
                    request_.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);

                    PrepareRequest(client_, request_, url_);

                    var response_ = await client_.SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse_ = true;
                    try
                    {
                        var headers_ = System.Linq.Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                        if (response_.Content != null && response_.Content.Headers != null)
                        {
                            foreach (var item_ in response_.Content.Headers)
                                headers_[item_.Key] = item_.Value;
                        }

                        ProcessResponse(client_, response_);

                        var status_ = (int)response_.StatusCode;
                        if (status_ == 200)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<StacFeatureCollection>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new StacApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }
                            return objectResponse_.Object;
                        }
                        else
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<StacFeatureCollection>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new StacApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }
                            throw new StacApiException<StacFeatureCollection>("An error occurred.", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                    }
                    finally
                    {
                        if (disposeResponse_)
                            response_.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient_)
                    client_.Dispose();
            }
        }

        protected struct ObjectResponseResult<T>
        {
            public ObjectResponseResult(T responseObject, string responseText)
            {
                this.Object = responseObject;
                this.Text = responseText;
            }

            public T Object { get; }

            public string Text { get; }
        }

        public bool ReadResponseAsString { get; set; }

        protected virtual async System.Threading.Tasks.Task<ObjectResponseResult<T>> ReadObjectResponseAsync<T>(System.Net.Http.HttpResponseMessage response, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers, System.Threading.CancellationToken cancellationToken)
        {
            if (response == null || response.Content == null)
            {
                return new ObjectResponseResult<T>(default(T), string.Empty);
            }

            if (ReadResponseAsString)
            {
                var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                try
                {
                    var typedBody = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responseText, JsonSerializerSettings);
                    return new ObjectResponseResult<T>(typedBody, responseText);
                }
                catch (Newtonsoft.Json.JsonException exception)
                {
                    var message = "Could not deserialize the response body string as " + typeof(T).FullName + ".";
                    throw new StacApiException(message, (int)response.StatusCode, responseText, headers, exception);
                }
            }
            else
            {
                try
                {
                    using (var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                    using (var streamReader = new System.IO.StreamReader(responseStream))
                    using (var jsonTextReader = new Newtonsoft.Json.JsonTextReader(streamReader))
                    {
                        var serializer = Newtonsoft.Json.JsonSerializer.Create(JsonSerializerSettings);
                        var typedBody = serializer.Deserialize<T>(jsonTextReader);
                        return new ObjectResponseResult<T>(typedBody, string.Empty);
                    }
                }
                catch (Newtonsoft.Json.JsonException exception)
                {
                    var message = "Could not deserialize the response body stream as " + typeof(T).FullName + ".";
                    throw new StacApiException(message, (int)response.StatusCode, string.Empty, headers, exception);
                }
            }
        }

        private string ConvertToString(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value == null)
            {
                return "";
            }

            if (value is System.Enum)
            {
                var name = System.Enum.GetName(value.GetType(), value);
                if (name != null)
                {
                    var field = System.Reflection.IntrospectionExtensions.GetTypeInfo(value.GetType()).GetDeclaredField(name);
                    if (field != null)
                    {
                        var attribute = System.Reflection.CustomAttributeExtensions.GetCustomAttribute(field, typeof(System.Runtime.Serialization.EnumMemberAttribute)) 
                            as System.Runtime.Serialization.EnumMemberAttribute;
                        if (attribute != null)
                        {
                            return attribute.Value != null ? attribute.Value : name;
                        }
                    }

                    var converted = System.Convert.ToString(System.Convert.ChangeType(value, System.Enum.GetUnderlyingType(value.GetType()), cultureInfo));
                    return converted == null ? string.Empty : converted;
                }
            }
            else if (value is bool) 
            {
                return System.Convert.ToString((bool)value, cultureInfo).ToLowerInvariant();
            }
            else if (value is byte[])
            {
                return System.Convert.ToBase64String((byte[]) value);
            }
            else if (value.GetType().IsArray)
            {
                var array = System.Linq.Enumerable.OfType<object>((System.Array) value);
                return string.Join(",", System.Linq.Enumerable.Select(array, o => ConvertToString(o, cultureInfo)));
            }

            var result = System.Convert.ToString(value, cultureInfo);
            return result == null ? "" : result;
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