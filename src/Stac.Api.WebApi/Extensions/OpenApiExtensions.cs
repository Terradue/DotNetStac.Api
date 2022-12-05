using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
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
                endpoints.MapGet(basePath + spec.Value.OpenApiPath.ToLower(), async context =>
                {
                    await WriteOpenApiAsync(context, spec.Value);
                }).RequireCors("All");
            }

            return endpoints;
        }

        private static async Task WriteOpenApiAsync(HttpContext context, OpenApiSpecification value)
        {
            HttpClient client = new HttpClient();
            context.Response.Headers.ContentType = "application/yaml";
            string document = await client.GetStringAsync(value.Url);
            await context.Response.WriteAsync(document);
        }

        public static SwaggerUi3Settings ConfigureSwaggerUi3(this SwaggerUi3Settings c, CodeGenOptions code)
        {
            string basePath = string.Format("/openapi/{0}/", code.ApiVersion);
            foreach ( var spec in code.Specifications)
            {
                c.SwaggerRoutes.Add(new SwaggerUi3Route(spec.Key.ToLower(), basePath + spec.Value.OpenApiPath.ToLower()));
            }
            
            return c;
        }
    }
}