using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using NJsonSchema;
using NJsonSchema.Generation;
using NJsonSchema.Yaml;
using NSwag;
using NSwag.CodeGeneration.CSharp;

namespace Stac.Api.CodeGen
{
    internal class ControllerCodeGen
    {
        private readonly IOptions<CodeGenOptions> options;

        public ControllerCodeGen(IOptions<CodeGenOptions> options)
        {
            this.options = options;
        }

        public async Task ExecuteAsync(string generatedCodeBasePath)
        {
            foreach (var spec in options.Value.Specifications)
            {
                string content = null;
                string documentPath = null;
                if (!string.IsNullOrEmpty(spec.Value.Url))
                {
                    HttpClient client = new HttpClient();
                    content = await client.GetStringAsync(spec.Value.Url);
                    documentPath = spec.Value.Url;
                }
                else if (!string.IsNullOrEmpty(spec.Value.File))
                {
                    content = await File.ReadAllTextAsync(spec.Value.File);
                    documentPath = spec.Value.File;
                }
                OpenApiDocument document = await OpenApiYamlDocument.FromYamlAsync(content, documentPath, SchemaType.OpenApi3, doc => GetResolver(doc, spec.Value.ExcludedSchemas));
                // JsonSchemaReferenceUtilities.UpdateSchemaReferencePaths(document, true, new DefaultContractResolver());
                string code = await GenerateCode(document, options.Value.GenerateControllerGeneratorSettings(spec.Key), spec.Value.ExcludedPaths);
                string path = Path.Join(generatedCodeBasePath, spec.Value.ControllerOutputFilePath);
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                File.WriteAllText(path, code);
            }
        }

        private JsonReferenceResolver GetResolver(OpenApiDocument arg, IEnumerable<string> excludedDefinitions)
        {
            var schemaResolver = new OpenApiSchemaResolver(arg, new JsonSchemaGeneratorSettings());
            return new StacReferenceResolver(schemaResolver, excludedDefinitions);
        }

        private async Task<string> GenerateCode(OpenApiDocument document, CSharpControllerGeneratorSettings settings, IEnumerable<string> excludedPaths)
        {
            if (excludedPaths != null)
            {
                foreach (var path in excludedPaths)
                {
                    document.Paths.Remove(path);
                }
            }

            FilterDocument(document);

            var generator = new CSharpControllerGenerator(document, settings);

            return generator.GenerateFile();
        }

        private void FilterDocument(OpenApiDocument document)
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