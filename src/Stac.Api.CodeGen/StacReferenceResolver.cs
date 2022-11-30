using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Namotion.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NJsonSchema;
using NJsonSchema.Infrastructure;
using NJsonSchema.References;
using NJsonSchema.Yaml;
using NSwag;
using YamlDotNet.Serialization;

namespace Stac.Api.CodeGen
{
    internal class StacReferenceResolver : JsonAndYamlReferenceResolver
    {
        private readonly JsonSchemaAppender _schemaAppender;
        private readonly Dictionary<string, IJsonReference> _resolvedObjects = new Dictionary<string, IJsonReference>();
        private readonly IEnumerable<string> _excludedDefinitions;
        private readonly IEnumerable<UrlMapping> _urlmappings;

        public StacReferenceResolver(JsonSchemaAppender schemaAppender, IEnumerable<string> excludedDefinitions, IEnumerable<UrlMapping> urlmappings) : base(schemaAppender)
        {
            _excludedDefinitions = excludedDefinitions;
            _urlmappings = urlmappings;
            _schemaAppender = schemaAppender;
        }

        public override IJsonReference ResolveDocumentReference(object rootObject, string jsonPath, Type targetType, IContractResolver contractResolver)
        {
            var allSegments = jsonPath.Split('/').Skip(1).ToList();
            // var schema = ResolveDocumentReference(rootObject, allSegments, targetType, contractResolver, new HashSet<object>());
            // if (schema == null)
            // {
            //     throw new InvalidOperationException("Could not resolve the path '" + jsonPath + "'.");
            // }

            // return schema;

            if (_excludedDefinitions.Contains(jsonPath) || (
                rootObject is JsonSchema schema && _excludedDefinitions.Contains(schema.DocumentPath + jsonPath)
            ))
            {
                return GetSimpleObjectSchema();
            }

            OpenApiDocument openApi = rootObject as OpenApiDocument;
            if (openApi == null && rootObject is OpenApiJsonSchemas openApiJsonSchemas)
            {
                openApi = openApiJsonSchemas.OpenApi;
            }

            if (openApi != null)
            {
                switch (allSegments[0])
                {
                    case "paths":
                        return openApi.Paths.FirstOrDefault(p => p.Key == allSegments[1]).Value;
                    case "components":
                        switch (allSegments[1])
                        {
                            case "schemas":
                                return GetSchema(allSegments, openApi);
                            case "responses":
                                return GetResponse(allSegments, openApi);
                            case "parameters":
                                return GetParameter(allSegments, openApi);
                            case "examples":
                                return openApi.Components.Examples.FirstOrDefault(e => e.Key == allSegments[2]).Value;
                        }
                        break;
                    case "definitions":
                        return openApi.Definitions.FirstOrDefault(p => p.Key == allSegments[1]).Value;
                }
                throw new InvalidOperationException("Could not resolve the path '" + jsonPath + "'.");
            }

            var refe = base.ResolveDocumentReference(rootObject, jsonPath, targetType, contractResolver);
            return refe;
        }

        private static IJsonReference GetResponse(List<string> allSegments, OpenApiDocument openApi)
        {
            var response = openApi.Components.Responses.FirstOrDefault(r => r.Key == allSegments[2]).Value;
            // This is a workaround for the fact that the OpenApiDocument does not have a reference to the parent document
            // and loose the name of the reponse when the reference is resolved.
            if (response.Schema != null && !response.Schema.HasTypeNameTitle)
            {
                response.Schema.Title = allSegments[2];
            }
            return response;
        }

        private static IJsonReference GetParameter(List<string> allSegments, OpenApiDocument openApi)
        {
            var parameter = openApi.Components.Parameters.FirstOrDefault(r => r.Key == allSegments[2]).Value;
            // This is a workaround for the fact that the OpenApiDocument does not have a reference to the parent document
            // and loose the name of the reponse when the reference is resolved.
            if (parameter.Schema != null && !parameter.Schema.HasTypeNameTitle)
            {
                parameter.Schema.Title = allSegments[2];
            }
            return parameter;
        }

        private static IJsonReference GetSchema(List<string> allSegments, OpenApiDocument openApi)
        {
            var response = openApi.Components.Schemas.FirstOrDefault(r => r.Key == allSegments[2]).Value;
            // This is a workaround for the fact that the OpenApiDocument does not have a reference to the parent document
            // and loose the name of the reponse when the reference is resolved.
            // if (response != null && !response.HasTypeNameTitle)
            // {
            //     response.Title = allSegments[2];
            // }
            return response;
        }

        private IJsonReference GetSimpleObjectSchema()
        {
            return JsonSchema.CreateAnySchema();
        }

        public override Task<IJsonReference> ResolveFileReferenceAsync(string filePath, CancellationToken cancellationToken = default)
        {
            return base.ResolveFileReferenceAsync(filePath, cancellationToken);
        }

        public override async Task<IJsonReference> ResolveUrlReferenceAsync(string url, CancellationToken cancellationToken = default)
        {
            var urlChange = _urlmappings.FirstOrDefault(u => u.Url == url);
            if (urlChange != null)
            {
                url = urlChange.UrlChange;
            }
            var data = await DynamicApis.HttpGetAsync(url, cancellationToken).ConfigureAwait(false);
            foreach ( var urlMap in _urlmappings)
            {
                data = data.Replace(urlMap.Url, urlMap.UrlChange);
            }
            var deserializer = new DeserializerBuilder().Build();
            var yamlObject = deserializer.Deserialize(new StringReader(data));
            var serializer = new SerializerBuilder()
                .JsonCompatible()
                .Build();

            var json = serializer.Serialize(yamlObject);
            var openapi = await OpenApiDocument.FromJsonAsync(json, url, SchemaType.OpenApi3, schema => this, cancellationToken).ConfigureAwait(false);
            var schema = await JsonSchema.FromJsonAsync(json, url, schema => this, cancellationToken);
            return new OpenApiJsonSchemas(openapi, url);
        }

        private async Task<IJsonReference> ResolveReferenceAsync(object rootObject, string jsonPath, Type targetType, IContractResolver contractResolver, bool append, CancellationToken cancellationToken = default)
        {
            if (jsonPath == "#")
            {
                if (rootObject is IJsonReference)
                {
                    return (IJsonReference)rootObject;
                }

                throw new InvalidOperationException("Could not resolve the JSON path '#' because the root object is not a JsonSchema4.");
            }
            else if (jsonPath.StartsWith("#/"))
            {
                return ResolveDocumentReference(rootObject, jsonPath, targetType, contractResolver);
            }
            else if (jsonPath.StartsWith("http://") || jsonPath.StartsWith("https://"))
            {
                return await ResolveUrlReferenceWithAlreadyResolvedCheckAsync(jsonPath, jsonPath, targetType, contractResolver, append, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                var documentPathProvider = rootObject as IDocumentPathProvider;

                var documentPath = documentPathProvider?.DocumentPath;
                if (documentPath != null)
                {
                    if (documentPath.StartsWith("http://") || documentPath.StartsWith("https://"))
                    {
                        var url = new Uri(new Uri(documentPath), jsonPath).ToString();
                        return await ResolveUrlReferenceWithAlreadyResolvedCheckAsync(url, jsonPath, targetType, contractResolver, append, cancellationToken).ConfigureAwait(false);
                    }
                    else
                    {
                        // Split the file path and fragment before concatenating with
                        // document path. If document path have '#' in it, doing this
                        // later would not work.
                        var filePath = ResolveFilePath(documentPath, jsonPath);
                        return await ResolveFileReferenceWithAlreadyResolvedCheckAsync(filePath, targetType, contractResolver, jsonPath, append, cancellationToken).ConfigureAwait(false);
                    }
                }
                else
                {
                    throw new NotSupportedException("Could not resolve the JSON path '" + jsonPath + "' because no document path is available.");
                }
            }
        }

        /// <summary>Resolves file path.</summary>
        /// <param name="documentPath">The document path.</param>
        /// <param name="jsonPath">The JSON path</param>
        public override string ResolveFilePath(string documentPath, string jsonPath)
        {
            // var arr = Regex.Split(jsonPath, @"(?=#)");
            // return DynamicApis.PathCombine(DynamicApis.PathGetDirectoryName(documentPath), arr[0]);
            return base.ResolveFilePath(documentPath, jsonPath);
        }

        private async Task<IJsonReference> ResolveFileReferenceWithAlreadyResolvedCheckAsync(string filePath, Type targetType, IContractResolver contractResolver, string jsonPath, bool append, CancellationToken cancellationToken)
        {
            try
            {
                var fullPath = DynamicApis.GetFullPath(filePath);
                var arr = Regex.Split(jsonPath, @"(?=#)");

                fullPath = DynamicApis.HandleSubdirectoryRelativeReferences(fullPath, jsonPath);

                if (!_resolvedObjects.ContainsKey(fullPath))
                {
                    var loadedFile = await ResolveFileReferenceAsync(fullPath).ConfigureAwait(false);
                    loadedFile.DocumentPath = arr[0];
                    _resolvedObjects[fullPath] = loadedFile;
                }

                var referencedFile = _resolvedObjects[fullPath];
                var resolvedSchema = arr.Length == 1 ? referencedFile : await ResolveReferenceAsync(referencedFile, arr[1], targetType, contractResolver).ConfigureAwait(false);
                if (resolvedSchema is JsonSchema && append &&
                    (_schemaAppender.RootObject as JsonSchema)?.Definitions.Values.Contains(referencedFile) != true)
                {
                    var key = jsonPath.Split('/', '\\').Last().Split('.').First();
                    _schemaAppender.AppendSchema((JsonSchema)resolvedSchema, key);
                }

                return resolvedSchema;
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("Could not resolve the JSON path '" + jsonPath + "' within the file path '" + filePath + "'.", exception);
            }
        }

        private async Task<IJsonReference> ResolveUrlReferenceWithAlreadyResolvedCheckAsync(string fullJsonPath, string jsonPath, Type targetType, IContractResolver contractResolver, bool append, CancellationToken cancellationToken)
        {
            try
            {
                var arr = fullJsonPath.Split('#');
                if (!_resolvedObjects.ContainsKey(arr[0]))
                {
                    var schema = await ResolveUrlReferenceAsync(arr[0], cancellationToken).ConfigureAwait(false);
                    schema.DocumentPath = arr[0];
                    if (schema is JsonSchema && append)
                    {
                        _schemaAppender.AppendSchema((JsonSchema)schema, null);
                    }

                    _resolvedObjects[arr[0]] = schema;
                }

                var result = _resolvedObjects[arr[0]];
                return arr.Length == 1 ? result : await ResolveReferenceAsync(result, "#" + arr[1], targetType, contractResolver, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("Could not resolve the JSON path '" + jsonPath + "' with the full JSON path '" + fullJsonPath + "'.", exception);
            }
        }

        private IJsonReference ResolveDocumentReference(object obj, List<string> segments, Type targetType, IContractResolver contractResolver, HashSet<object> checkedObjects)
        {
            if (obj == null || obj is string || checkedObjects.Contains(obj))
            {
                return null;
            }

            if (obj is IJsonReference reference && reference.Reference != null)
            {
                var result = ResolveDocumentReferenceWithoutDereferencing(reference.Reference, segments, targetType, contractResolver, checkedObjects);
                if (result == null)
                {
                    return ResolveDocumentReferenceWithoutDereferencing(obj, segments, targetType, contractResolver, checkedObjects);
                }
                else
                {
                    return result;
                }
            }

            return ResolveDocumentReferenceWithoutDereferencing(obj, segments, targetType, contractResolver, checkedObjects);
        }

        private IJsonReference ResolveDocumentReferenceWithoutDereferencing(object obj, List<string> segments, Type targetType, IContractResolver contractResolver, HashSet<object> checkedObjects)
        {
            if (segments.Count == 0)
            {
                if (obj is IDictionary)
                {
                    var settings = new JsonSerializerSettings { ContractResolver = contractResolver };
                    var json = JsonConvert.SerializeObject(obj, settings);
                    return (IJsonReference)JsonConvert.DeserializeObject(json, targetType, settings);
                }
                else
                {
                    return (IJsonReference)obj;
                }
            }

            checkedObjects.Add(obj);
            var firstSegment = segments[0];

            if (obj is IDictionary dictionary)
            {
                if (dictionary.Contains(firstSegment))
                {
                    return ResolveDocumentReference(dictionary[firstSegment], segments.Skip(1).ToList(), targetType, contractResolver, checkedObjects);
                }
            }
            else if (obj is IEnumerable)
            {
                int index;
                if (int.TryParse(firstSegment, out index))
                {
                    var enumerable = ((IEnumerable)obj).Cast<object>().ToArray();
                    if (enumerable.Length > index)
                    {
                        return ResolveDocumentReference(enumerable[index], segments.Skip(1).ToList(), targetType, contractResolver, checkedObjects);
                    }
                }
            }
            else
            {
                var extensionObj = obj as IJsonExtensionObject;
                if (extensionObj?.ExtensionData?.ContainsKey(firstSegment) == true)
                {
                    return ResolveDocumentReference(extensionObj.ExtensionData[firstSegment], segments.Skip(1).ToList(), targetType, contractResolver, checkedObjects);
                }

                foreach (var member in obj
                    .GetType()
                    .GetContextualAccessors()
                    .Where(p => p.AccessorType.GetInheritedAttribute<JsonIgnoreAttribute>() == null))
                {
                    var pathSegment = member.Name;
                    if (pathSegment == firstSegment)
                    {
                        var value = member.GetValue(obj);
                        return ResolveDocumentReference(value, segments.Skip(1).ToList(), targetType, contractResolver, checkedObjects);
                    }
                }
            }

            return null;
        }
    }
}