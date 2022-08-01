using NSwag.AspNetCore;
using Stac.Api.CodeGen;

namespace Stac.Api.WebApi.Extensions
{
    public static class OpenApiExtensions
    {
        public static IEndpointRouteBuilder UseOpenApi(this IEndpointRouteBuilder endpoints, CodeGenOptions code)
        {
            string basePath = string.Format("/openapi/{0}/", code.ApiVersion);
            foreach ( var spec in code.Specifications)
            {
                endpoints.MapGet(basePath + spec.Value.OpenApiPath, async context =>
                {
                    await WriteOpenApiAsync(context, spec.Value);
                });
            }

            return endpoints;
        }

        private static async Task WriteOpenApiAsync(HttpContext context, OpenApiSpecification value)
        {
            context.Response.Headers.ContentType = "application/yaml";
            NSwag.OpenApiDocument document = await NSwag.OpenApiYamlDocument.FromUrlAsync(value.Url);
            await context.Response.WriteAsync(NSwag.OpenApiYamlDocument.ToYaml(document));
        }

        public static SwaggerUi3Settings ConfigureSwaggerUi3(this SwaggerUi3Settings c, CodeGenOptions code)
        {
            string basePath = string.Format("/openapi/{0}/", code.ApiVersion);
            foreach ( var spec in code.Specifications)
            {
                c.SwaggerRoutes.Add(new SwaggerUi3Route(spec.Key, basePath + spec.Value.OpenApiPath));
            }
            
            return c;
        }
    }
}