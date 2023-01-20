using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Schema;
using Stac.Api.Interfaces;

namespace Stac.Api.Services.Queryable
{
    public class StacQueryablesOptions : JSchema
    {
        public static StacQueryablesOptions GenerateDefaultOptions<T>(IStacApiContext stacApiContext) where T : IStacObject
        {
            var schema = new StacQueryablesOptions()
            {
                Id = new System.Uri(stacApiContext.BaseUri, "queryables"),
                Type = JSchemaType.Object,
                Title = "STAC Item Queryables",
                Description = "Queryable names for STAC Items",
            };
            schema.Properties.Add("id", new JSchema()
            {
                Reference = new System.Uri("https://schemas.stacspec.org/v1.0.0/item-spec/json-schema/item.json#/id"),
                Description = "The STAC Item ID",
            });
            schema.Properties.Add("collection", new JSchema()
            {
                Reference = new System.Uri("https://schemas.stacspec.org/v1.0.0/item-spec/json-schema/item.json#/collection"),
                Description = "The STAC Item Collection",
            });
            schema.Properties.Add("geometry", new JSchema()
            {
                Reference = new System.Uri("https://schemas.stacspec.org/v1.0.0/item-spec/json-schema/item.json#/geometry"),
                Description = "The STAC Item Geometry",
            });
            schema.Properties.Add("datetime", new JSchema()
            {
                Reference = new System.Uri("https://schemas.stacspec.org/v1.0.0/item-spec/json-schema/item.json#/properties/datetime"),
                Description = "The STAC Item Datetime",
            });
            return schema;
        }
    }
}