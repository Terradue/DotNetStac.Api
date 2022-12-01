//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

using Stac;
using Stac.Common;
using Stac.Api.Models;
using Stac.Api.Clients.Collections;

#pragma warning disable 108 // Disable "CS0108 '{derivedDto}.ToJson()' hides inherited member '{dtoBase}.ToJson()'. Use the new keyword if hiding was intended."
#pragma warning disable 114 // Disable "CS0114 '{derivedDto}.RaisePropertyChanged(String)' hides inherited member 'dtoBase.RaisePropertyChanged(String)'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword."
#pragma warning disable 472 // Disable "CS0472 The result of the expression is always 'false' since a value of type 'Int32' is never equal to 'null' of type 'Int32?'
#pragma warning disable 1573 // Disable "CS1573 Parameter '...' has no matching param tag in the XML comment for ...
#pragma warning disable 1591 // Disable "CS1591 Missing XML comment for publicly visible type or member ..."
#pragma warning disable 8073 // Disable "CS8073 The result of the expression is always 'false' since a value of type 'T' is never equal to 'null' of type 'T?'"
#pragma warning disable 3016 // Disable "CS3016 Arrays as attribute arguments is not CLS-compliant"
#pragma warning disable 8603 // Disable "CS8603 Possible null reference return"

namespace Stac.Api.WebApi.Controllers.Collections
{
    using System = global::System;

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public interface ICollectionsController
    {

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

        System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<StacCollections>> GetCollectionsAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

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

        System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<StacCollection>> DescribeCollectionAsync(string collectionId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

    }

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.16.1.0 (NJsonSchema v10.7.2.0 (Newtonsoft.Json v13.0.0.0))")]

    public partial class CollectionsController : Stac.Api.WebApi.StacApiController
    {
        private ICollectionsController _implementation;

        public CollectionsController(ICollectionsController implementation)
        {
            _implementation = implementation;
        }

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
        [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.Route("collections")]
        public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<StacCollections>> GetCollections(System.Threading.CancellationToken cancellationToken)
        {

            return _implementation.GetCollectionsAsync(cancellationToken);
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
        [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.Route("collections/{collectionId}")]
        public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.ActionResult<StacCollection>> DescribeCollection([Microsoft.AspNetCore.Mvc.ModelBinding.BindRequired] string collectionId, System.Threading.CancellationToken cancellationToken)
        {

            return _implementation.DescribeCollectionAsync(collectionId, cancellationToken);
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