using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Stac.Api.Converters;
using Stac.Api.Interfaces;

namespace Stac.Api.Models
{
    public class StacQueryables
    {
        [JsonProperty("$schema")]
        [JsonConverter(typeof(UriConverter))]
        public Uri SchemaVersion { get; set; }

        [JsonProperty("$id")]
        [JsonConverter(typeof(UriConverter))]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("properties")]
        public IDictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();

        public static StacQueryables GenerateDefaultOptions<T>(IStacApiContext stacApiContext) where T : IStacObject
        {
            var schema = new StacQueryables()
            {
                SchemaVersion = new Uri("https://json-schema.org/draft/2019-09/schema"),
                Id = new System.Uri(stacApiContext.BaseUri, "queryables").ToString(),
                Type = "object",
                Title = "STAC Item Queryables",
                Description = "Queryable names for STAC Items",
            };
            schema.Properties.Add("id", new StacQueryablesProperty()
            {
                Reference = new System.Uri("https://schemas.stacspec.org/v1.0.0/item-spec/json-schema/item.json#/id"),
                Description = "The STAC Item ID",
            });
            schema.Properties.Add("collection", new StacQueryablesProperty()
            {
                Reference = new System.Uri("https://schemas.stacspec.org/v1.0.0/item-spec/json-schema/item.json#/collection"),
                Description = "The STAC Item Collection",
            });
            schema.Properties.Add("geometry", new StacQueryablesProperty()
            {
                Reference = new System.Uri("https://schemas.stacspec.org/v1.0.0/item-spec/json-schema/item.json#/geometry"),
                Description = "The STAC Item Geometry",
            });
            schema.Properties.Add("datetime", new StacQueryablesProperty()
            {
                Reference = new System.Uri("https://schemas.stacspec.org/v1.0.0/item-spec/json-schema/item.json#/properties/datetime"),
                Description = "The STAC Item Datetime",
            });
            return schema;
        }
    }

    public class StacQueryablesProperty
    {
        public StacQueryablesProperty()
        {
        }

        public StacQueryablesProperty(Uri reference, string description)
        {
            Reference = reference;
            Description = description;
        }

        [JsonProperty("$ref")]
        [JsonConverter(typeof(UriConverter))]
        public Uri Reference { get; set; }

        public string Description { get; set; }
    }
}