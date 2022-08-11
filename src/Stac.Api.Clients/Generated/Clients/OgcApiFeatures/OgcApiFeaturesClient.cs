//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

using Stac;
using Stac.Api.Models;
using Stac.Api.WebApi.Controllers.Collections;
using Stac.Api.WebApi.Controllers.Core;
using Stac.Api.WebApi.Controllers.Fragments.Fields;
using Stac.Api.WebApi.Controllers.Fragments.Filter;
using Stac.Api.WebApi.Controllers.Fragments.Sort;
using Stac.Api.WebApi.Controllers.ItemSearch;
using Stac.Api.WebApi.Controllers.OgcApiFeatures;

#pragma warning disable 108 // Disable "CS0108 '{derivedDto}.ToJson()' hides inherited member '{dtoBase}.ToJson()'. Use the new keyword if hiding was intended."
#pragma warning disable 114 // Disable "CS0114 '{derivedDto}.RaisePropertyChanged(String)' hides inherited member 'dtoBase.RaisePropertyChanged(String)'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword."
#pragma warning disable 472 // Disable "CS0472 The result of the expression is always 'false' since a value of type 'Int32' is never equal to 'null' of type 'Int32?'
#pragma warning disable 1573 // Disable "CS1573 Parameter '...' has no matching param tag in the XML comment for ...
#pragma warning disable 1591 // Disable "CS1591 Missing XML comment for publicly visible type or member ..."
#pragma warning disable 8073 // Disable "CS8073 The result of the expression is always 'false' since a value of type 'T' is never equal to 'null' of type 'T?'"
#pragma warning disable 3016 // Disable "CS3016 Arrays as attribute arguments is not CLS-compliant"
#pragma warning disable 8603 // Disable "CS8603 Possible null reference return"

namespace Stac.Api.Clients.OgcApiFeatures
{
    using System = global::System;

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class OgcApiFeaturesClient : Stac.Api.Clients.StacApiClient
    {
        private System.Net.Http.HttpClient _httpClient;
        private System.Lazy<Newtonsoft.Json.JsonSerializerSettings> _settings;

        public OgcApiFeaturesClient(string baseUrl, System.Net.Http.HttpClient httpClient)
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
        /// information about specifications that this API conforms to
        /// </summary>
        /// <returns>The URIs of all conformance classes supported by the server.
        /// <br/>
        /// <br/>To support "generic" clients that want to access multiple
        /// <br/>OGC API Features implementations - and not "just" a specific
        /// <br/>API / server, the server declares the conformance
        /// <br/>classes it implements and conforms to.</returns>
        /// <exception cref="StacApiException">A server side error occurred.</exception>
        public virtual System.Threading.Tasks.Task<ConformanceClasses> GetConformanceDeclarationAsync()
        {
            return GetConformanceDeclarationAsync(System.Threading.CancellationToken.None);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>
        /// information about specifications that this API conforms to
        /// </summary>
        /// <returns>The URIs of all conformance classes supported by the server.
        /// <br/>
        /// <br/>To support "generic" clients that want to access multiple
        /// <br/>OGC API Features implementations - and not "just" a specific
        /// <br/>API / server, the server declares the conformance
        /// <br/>classes it implements and conforms to.</returns>
        /// <exception cref="StacApiException">A server side error occurred.</exception>
        public virtual async System.Threading.Tasks.Task<ConformanceClasses> GetConformanceDeclarationAsync(System.Threading.CancellationToken cancellationToken)
        {
            var urlBuilder_ = new System.Text.StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/conformance");

            var client_ = _httpClient;
            var disposeClient_ = false;
            try
            {
                using (var request_ = new System.Net.Http.HttpRequestMessage())
                {
                    request_.Method = new System.Net.Http.HttpMethod("GET");
                    request_.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));

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
                            var objectResponse_ = await ReadObjectResponseAsync<ConformanceClasses>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new StacApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }
                            return objectResponse_.Object;
                        }
                        else
                        if (status_ == 500)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<Stac.Api.WebApi.Controllers.Core.ExceptionInfo>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new StacApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }
                            throw new StacApiException<Stac.Api.WebApi.Controllers.Core.ExceptionInfo>("A server error occurred.", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        {
                            var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new StacApiException("The HTTP status code of the response was not expected (" + status_ + ").", status_, responseData_, headers_, null);
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
        /// <exception cref="StacApiException">A server side error occurred.</exception>
        public virtual System.Threading.Tasks.Task<StacFeatureCollection> GetFeaturesAsync(string collectionId, int? limit, string bbox, string datetime)
        {
            return GetFeaturesAsync(collectionId, limit, bbox, datetime, System.Threading.CancellationToken.None);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
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
        /// <exception cref="StacApiException">A server side error occurred.</exception>
        public virtual async System.Threading.Tasks.Task<StacFeatureCollection> GetFeaturesAsync(string collectionId, int? limit, string bbox, string datetime, System.Threading.CancellationToken cancellationToken)
        {
            if (collectionId == null)
                throw new System.ArgumentNullException("collectionId");

            var urlBuilder_ = new System.Text.StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/collections/{collectionId}/items?");
            urlBuilder_.Replace("{collectionId}", System.Uri.EscapeDataString(ConvertToString(collectionId, System.Globalization.CultureInfo.InvariantCulture)));
            if (limit != null)
            {
                urlBuilder_.Append(System.Uri.EscapeDataString("limit") + "=").Append(System.Uri.EscapeDataString(ConvertToString(limit, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            }
            if (bbox != null)
            {
                urlBuilder_.Append(System.Uri.EscapeDataString("bbox") + "=").Append(System.Uri.EscapeDataString(ConvertToString(bbox, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
            }
            if (datetime != null)
            {
                urlBuilder_.Append(System.Uri.EscapeDataString("datetime") + "=").Append(System.Uri.EscapeDataString(ConvertToString(datetime, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
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
                        if (status_ == 400)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<Stac.Api.WebApi.Controllers.Core.ExceptionInfo>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new StacApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }
                            throw new StacApiException<Stac.Api.WebApi.Controllers.Core.ExceptionInfo>("A query parameter has an invalid value.", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        if (status_ == 404)
                        {
                            string responseText_ = ( response_.Content == null ) ? string.Empty : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new StacApiException("The requested URI was not found.", status_, responseText_, headers_, null);
                        }
                        else
                        if (status_ == 500)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<Stac.Api.WebApi.Controllers.Core.ExceptionInfo>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new StacApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }
                            throw new StacApiException<Stac.Api.WebApi.Controllers.Core.ExceptionInfo>("A server error occurred.", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        {
                            var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new StacApiException("The HTTP status code of the response was not expected (" + status_ + ").", status_, responseData_, headers_, null);
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
        /// fetch a single feature
        /// </summary>
        /// <param name="collectionId">local identifier of a collection</param>
        /// <param name="featureId">local identifier of a feature</param>
        /// <returns>fetch the feature with id `featureId` in the feature collection
        /// <br/>with id `collectionId`</returns>
        /// <exception cref="StacApiException">A server side error occurred.</exception>
        public virtual System.Threading.Tasks.Task<StacItem> GetFeatureAsync(string collectionId, string featureId)
        {
            return GetFeatureAsync(collectionId, featureId, System.Threading.CancellationToken.None);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>
        /// fetch a single feature
        /// </summary>
        /// <param name="collectionId">local identifier of a collection</param>
        /// <param name="featureId">local identifier of a feature</param>
        /// <returns>fetch the feature with id `featureId` in the feature collection
        /// <br/>with id `collectionId`</returns>
        /// <exception cref="StacApiException">A server side error occurred.</exception>
        public virtual async System.Threading.Tasks.Task<StacItem> GetFeatureAsync(string collectionId, string featureId, System.Threading.CancellationToken cancellationToken)
        {
            if (collectionId == null)
                throw new System.ArgumentNullException("collectionId");

            if (featureId == null)
                throw new System.ArgumentNullException("featureId");

            var urlBuilder_ = new System.Text.StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/collections/{collectionId}/items/{featureId}");
            urlBuilder_.Replace("{collectionId}", System.Uri.EscapeDataString(ConvertToString(collectionId, System.Globalization.CultureInfo.InvariantCulture)));
            urlBuilder_.Replace("{featureId}", System.Uri.EscapeDataString(ConvertToString(featureId, System.Globalization.CultureInfo.InvariantCulture)));

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
                            var objectResponse_ = await ReadObjectResponseAsync<StacItem>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new StacApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }
                            return objectResponse_.Object;
                        }
                        else
                        if (status_ == 404)
                        {
                            string responseText_ = ( response_.Content == null ) ? string.Empty : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new StacApiException("The requested URI was not found.", status_, responseText_, headers_, null);
                        }
                        else
                        if (status_ == 500)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<Stac.Api.WebApi.Controllers.Core.ExceptionInfo>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new StacApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }
                            throw new StacApiException<Stac.Api.WebApi.Controllers.Core.ExceptionInfo>("A server error occurred.", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
                        }
                        else
                        {
                            var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new StacApiException("The HTTP status code of the response was not expected (" + status_ + ").", status_, responseData_, headers_, null);
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