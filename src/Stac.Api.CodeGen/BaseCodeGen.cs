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

            // Remove all the error responses schema from API operations
            // Otherwise the response type is a generic Response object
            try
            {
                foreach (var path in document.Paths.Values)
                {
                    foreach (var op in path.Values)
                    {
                        // op.Responses.Remove("400");
                        // op.Responses.Remove("404");
                        // op.Responses.Remove("500");
                        foreach (var response in op.Responses.Values)
                        {
                            if (response.ActualResponse.Schema.ActualSchema.Title == null)
                            {
                                response.ActualResponse.Schema.ActualSchema.Title = response.ActualResponse.Schema.Title;
                            }
                        }
                    }
                }
            }
            catch { }

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

            // Change Children Schema name (https://github.com/radiantearth/stac-api-spec/blob/v1.0.0-rc.1/children/openapi.yaml#L88)
            try
            {
                OpenApiOperation childrenOperation = document.Paths["/children"]["get"];
                OpenApiResponse childrenResponse = document.Components.Responses["Children"];
                JsonSchema childrenSchema = document.Components.Schemas["children"];

                document.Components.Responses.Remove("Children");
                document.Components.Schemas.Remove("children");
                document.Components.Schemas.Add("stacChildren", childrenSchema);
                childrenResponse.Content["application/json"].Schema.Reference = childrenSchema;
                document.Components.Responses.Add("StacChildren", childrenResponse);

                if (childrenOperation.Tags.Contains("Children"))
                {
                    childrenOperation.Responses["200"] = childrenResponse;
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