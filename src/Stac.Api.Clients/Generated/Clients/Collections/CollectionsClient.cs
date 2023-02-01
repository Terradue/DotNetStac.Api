//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

using Stac;
using Stac.Common;
using Stac.Api.Models;
using Stac.Api.Interfaces;

#pragma warning disable 108 // Disable "CS0108 '{derivedDto}.ToJson()' hides inherited member '{dtoBase}.ToJson()'. Use the new keyword if hiding was intended."
#pragma warning disable 114 // Disable "CS0114 '{derivedDto}.RaisePropertyChanged(String)' hides inherited member 'dtoBase.RaisePropertyChanged(String)'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword."
#pragma warning disable 472 // Disable "CS0472 The result of the expression is always 'false' since a value of type 'Int32' is never equal to 'null' of type 'Int32?'
#pragma warning disable 1573 // Disable "CS1573 Parameter '...' has no matching param tag in the XML comment for ...
#pragma warning disable 1591 // Disable "CS1591 Missing XML comment for publicly visible type or member ..."
#pragma warning disable 8073 // Disable "CS8073 The result of the expression is always 'false' since a value of type 'T' is never equal to 'null' of type 'T?'"
#pragma warning disable 3016 // Disable "CS3016 Arrays as attribute arguments is not CLS-compliant"
#pragma warning disable 8603 // Disable "CS8603 Possible null reference return"

namespace Stac.Api.Clients.Collections
{
    using System = global::System;

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class CollectionsClient : Stac.Api.Clients.StacApiClient
    {
        private System.Net.Http.HttpClient _httpClient;
        private System.Lazy<Newtonsoft.Json.JsonSerializerSettings> _settings;

        public CollectionsClient(System.Net.Http.HttpClient httpClient)
        {
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
        /// the feature collections in the dataset
        /// </summary>
        /// <returns>The feature collections shared by this API.
        /// <br/>
        /// <br/>The dataset is organized as one or more feature collections. This resource
        /// <br/>provides information about and access to the collections.
        /// <br/>
        /// <br/>The response contains the list of collections. For each collection, a link
        /// <br/>to the items in the collection (path `/collections/{collectionId}/items`,
        /// <br/>link relation `items`) as well as key information about the collection.
        /// <br/>This information includes:
        /// <br/>
        /// <br/>* A local identifier for the collection that is unique for the dataset;
        /// <br/>* A list of coordinate reference systems (CRS) in which geometries may be returned by the server. The first CRS is the default coordinate reference system (the default is always WGS 84 with axis order longitude/latitude);
        /// <br/>* An optional title and description for the collection;
        /// <br/>* An optional extent that can be used to provide an indication of the spatial and temporal extent of the collection - typically derived from the data;
        /// <br/>* An optional indicator about the type of the items in the collection (the default value, if the indicator is not provided, is 'feature').</returns>
        /// <exception cref="StacApiException">A server side error occurred.</exception>
        public virtual System.Threading.Tasks.Task<StacCollections> GetCollectionsAsync()
        {
            return GetCollectionsAsync(System.Threading.CancellationToken.None);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>
        /// the feature collections in the dataset
        /// </summary>
        /// <returns>The feature collections shared by this API.
        /// <br/>
        /// <br/>The dataset is organized as one or more feature collections. This resource
        /// <br/>provides information about and access to the collections.
        /// <br/>
        /// <br/>The response contains the list of collections. For each collection, a link
        /// <br/>to the items in the collection (path `/collections/{collectionId}/items`,
        /// <br/>link relation `items`) as well as key information about the collection.
        /// <br/>This information includes:
        /// <br/>
        /// <br/>* A local identifier for the collection that is unique for the dataset;
        /// <br/>* A list of coordinate reference systems (CRS) in which geometries may be returned by the server. The first CRS is the default coordinate reference system (the default is always WGS 84 with axis order longitude/latitude);
        /// <br/>* An optional title and description for the collection;
        /// <br/>* An optional extent that can be used to provide an indication of the spatial and temporal extent of the collection - typically derived from the data;
        /// <br/>* An optional indicator about the type of the items in the collection (the default value, if the indicator is not provided, is 'feature').</returns>
        /// <exception cref="StacApiException">A server side error occurred.</exception>
        public virtual async System.Threading.Tasks.Task<StacCollections> GetCollectionsAsync(System.Threading.CancellationToken cancellationToken)
        {
            var urlBuilder_ = new System.Text.StringBuilder();
            urlBuilder_.Append("collections");

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
                            var objectResponse_ = await ReadObjectResponseAsync<StacCollections>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new StacApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }
                            return objectResponse_.Object;
                        }
                        else
                        if (status_ == 500)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<Stac.Api.Clients.Features.ServerError>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new StacApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }
                            throw new StacApiException<Stac.Api.Clients.Features.ServerError>("A server error occurred.", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
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
        /// describe the feature collection with id `collectionId`
        /// </summary>
        /// <param name="collectionId">local identifier of a collection</param>
        /// <returns>Information about the feature collection with id `collectionId`.
        /// <br/>
        /// <br/>The response contains a link to the items in the collection
        /// <br/>(path `/collections/{collectionId}/items`, link relation `items`)
        /// <br/>as well as key information about the collection. This information
        /// <br/>includes:
        /// <br/>
        /// <br/>* A local identifier for the collection that is unique for the dataset;
        /// <br/>* A list of coordinate reference systems (CRS) in which geometries may be returned by the server. The first CRS is the default coordinate reference system (the default is always WGS 84 with axis order longitude/latitude);
        /// <br/>* An optional title and description for the collection;
        /// <br/>* An optional extent that can be used to provide an indication of the spatial and temporal extent of the collection - typically derived from the data;
        /// <br/>* An optional indicator about the type of the items in the collection (the default value, if the indicator is not provided, is 'feature').</returns>
        /// <exception cref="StacApiException">A server side error occurred.</exception>
        public virtual System.Threading.Tasks.Task<StacCollection> DescribeCollectionAsync(string collectionId)
        {
            return DescribeCollectionAsync(collectionId, System.Threading.CancellationToken.None);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>
        /// describe the feature collection with id `collectionId`
        /// </summary>
        /// <param name="collectionId">local identifier of a collection</param>
        /// <returns>Information about the feature collection with id `collectionId`.
        /// <br/>
        /// <br/>The response contains a link to the items in the collection
        /// <br/>(path `/collections/{collectionId}/items`, link relation `items`)
        /// <br/>as well as key information about the collection. This information
        /// <br/>includes:
        /// <br/>
        /// <br/>* A local identifier for the collection that is unique for the dataset;
        /// <br/>* A list of coordinate reference systems (CRS) in which geometries may be returned by the server. The first CRS is the default coordinate reference system (the default is always WGS 84 with axis order longitude/latitude);
        /// <br/>* An optional title and description for the collection;
        /// <br/>* An optional extent that can be used to provide an indication of the spatial and temporal extent of the collection - typically derived from the data;
        /// <br/>* An optional indicator about the type of the items in the collection (the default value, if the indicator is not provided, is 'feature').</returns>
        /// <exception cref="StacApiException">A server side error occurred.</exception>
        public virtual async System.Threading.Tasks.Task<StacCollection> DescribeCollectionAsync(string collectionId, System.Threading.CancellationToken cancellationToken)
        {
            if (collectionId == null)
                throw new System.ArgumentNullException("collectionId");

            var urlBuilder_ = new System.Text.StringBuilder();
            urlBuilder_.Append("collections/{collectionId}");
            urlBuilder_.Replace("{collectionId}", System.Uri.EscapeDataString(ConvertToString(collectionId, System.Globalization.CultureInfo.InvariantCulture)));

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
                            var objectResponse_ = await ReadObjectResponseAsync<StacCollection>(response_, headers_, cancellationToken).ConfigureAwait(false);
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
                            var objectResponse_ = await ReadObjectResponseAsync<Stac.Api.Clients.Features.ServerError>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new StacApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }
                            throw new StacApiException<Stac.Api.Clients.Features.ServerError>("A server error occurred.", status_, objectResponse_.Text, headers_, objectResponse_.Object, null);
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

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class StacCollections
    {
        [Newtonsoft.Json.JsonProperty("links", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public Links Links { get; set; } = new Links();

        [Newtonsoft.Json.JsonProperty("collections", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public System.Collections.Generic.ICollection<StacCollection> Collections { get; set; } = new System.Collections.ObjectModel.Collection<StacCollection>();

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties = new System.Collections.Generic.Dictionary<string, object>();

        [Newtonsoft.Json.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class Links : System.Collections.ObjectModel.Collection<StacApiLink>
    {

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class StacItem
    {

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