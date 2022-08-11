using System.Collections.Generic;
using System.Linq;
using NJsonSchema;
using NJsonSchema.Generation;
using NSwag;

namespace Stac.Api.CodeGen
{
    internal class BaseCodeGen
    {
        protected JsonReferenceResolver GetResolver(OpenApiDocument arg, IEnumerable<string> excludedDefinitions)
        {
            var schemaResolver = new OpenApiSchemaResolver(arg, new JsonSchemaGeneratorSettings());
            return new StacReferenceResolver(schemaResolver, excludedDefinitions);
        }

        protected void FilterDocument(OpenApiDocument document)
        {
            // Fix unsupported array for bbox (https://github.com/radiantearth/stac-api-spec/blob/v1.0.0-rc.1/item-search/openapi.yaml#L179)
            // Replace by a simple string
            if (document.Components.Parameters.ContainsKey("bbox"))
            {
                var bboxParam = document.Components.Parameters["bbox"];
                if (bboxParam.Schema.Type == JsonObjectType.Array)
                {
                    bboxParam.Schema.Type = JsonObjectType.String;
                    bboxParam.Schema.OneOf.Clear();
                    bboxParam.Schema.Items.Clear();
                }
            }

            // Set filter parameter with new name (https://github.com/radiantearth/stac-api-spec/blob/master/fragments/filter/openapi.yaml#L95)
            try
            {
                OpenApiParameter filterParam = document.Paths["/search"]?["get"]?.Parameters.Cast<OpenApiParameter>().FirstOrDefault(p => ((OpenApiParameter)p.Reference).Name == "filter");
                if (filterParam != null)
                {
                    var filterSchema = filterParam.Reference as OpenApiParameter;
                    OpenApiParameter newFilterParam = Clone(filterSchema);
                    newFilterParam.Name = "FilterParameter";
                    document.Paths["/search"]?["get"]?.Parameters.Remove(filterParam);
                    document.Paths["/search"]?["get"]?.Parameters.Add(newFilterParam);
                }
            }
            catch { }

            // Set fields parameter as query string (https://github.com/radiantearth/stac-api-spec/blob/v1.0.0-rc.1/fragments/fields/openapi.yaml#L9)
            // try
            // {
            //     OpenApiParameter fieldsParam = document.Paths["/search"]?["get"]?.Parameters.Cast<OpenApiParameter>().FirstOrDefault(p => ((OpenApiParameter)p.Reference).Name == "fields");
            //     if (fieldsParam != null)
            //     {
            //         var fieldsSchema = fieldsParam.Reference as OpenApiParameter;
            //         fieldsSchema.Name = "FieldsQueryString";
            //         fieldsSchema.Schema.Type = JsonObjectType.Object;
            //         fieldsSchema.Schema.Items.Add
            //     }
            // }
            // catch { }

            // Set sortby parameter as query string (https://github.com/radiantearth/stac-api-spec/blob/v1.0.0-rc.1/fragments/fields/openapi.yaml#L9)
            // try
            // {
            //     OpenApiParameter fieldsParam = document.Paths["/search"]?["get"]?.Parameters.Cast<OpenApiParameter>().FirstOrDefault(p => ((OpenApiParameter)p.Reference).Name == "sortby");
            //     if (fieldsParam != null)
            //     {
            //         var fieldsSchema = fieldsParam.Reference as OpenApiParameter;
            //         fieldsSchema.Name = "SortByQueryString";
            //         fieldsSchema.Schema.Type = JsonObjectType.Object;
            //     }
            // }
            // catch { }

            // Set intersects parameter as string (https://github.com/radiantearth/stac-api-spec/blob/v1.0.0-rc.1/item-search/openapi.yaml#L207)
            try
            {
                OpenApiParameter intersectsParam = document.Paths["/search"]?["get"]?.Parameters.Cast<OpenApiParameter>().FirstOrDefault(p => ((OpenApiParameter)p.Reference).Name == "intersects");
                if (intersectsParam != null)
                {
                    var intersectsSchema = intersectsParam.Reference as OpenApiParameter;
                    intersectsSchema.Name = "IntersectsQueryString";
                    intersectsSchema.Schema.Type = JsonObjectType.Object;
                }
            }
            catch { }

            // Remove first reference of the search response (https://github.com/radiantearth/stac-api-spec/blob/v1.0.0-rc.1/item-search/openapi.yaml#L52)
            // This is a Stac Feature Collection already implemented
             try
            {
                JsonSchema responseSchema = document.Paths["/search"]?["get"]?.Responses["200"].Content["application/geo+json"].Schema;
                if (responseSchema != null)
                {
                    var schemaRef = responseSchema.AllOf.FirstOrDefault(r => r.Reference.Description.Contains("A GeoJSON FeatureCollection"));
                    responseSchema.AllOf.Remove(schemaRef);
                }
            }
            catch { }

            // Set JsonSchema as return Type for Queryables (https://github.com/radiantearth/stac-api-spec/blob/v1.0.0-rc.1/fragments/filter/openapi.yaml#L167)
            try
            {
                OpenApiResponse response = document.Components.Responses.FirstOrDefault(s => s.Key == "Queryables").Value;
                if (response != null)
                {
                    
                    JsonSchema responseSchema = new JsonSchema();
                    responseSchema.Type = JsonObjectType.Object;
                    responseSchema.Items.Add(new JsonSchema
                    {
                        Type = JsonObjectType.String,
                        Id = "$id"
                    });
                    response.Content["application/schema+json"].Schema.Reference = responseSchema;
                    document.Components.Schemas.Add("JsonSchema", responseSchema);
                }
            }
            catch { }

        }

        private static OpenApiParameter Clone(OpenApiParameter filterParam)
        {
            return new OpenApiParameter()
            {
                Name = filterParam.Name,
                Schema = filterParam.Schema,
                Description = filterParam.Description,
                IsRequired = filterParam.IsRequired,
                Reference = filterParam.Reference,
            };
        }
    }
}