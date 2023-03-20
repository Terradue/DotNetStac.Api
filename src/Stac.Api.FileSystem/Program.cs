using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Options;
using Stac.Api.CodeGen;
using Stac.Api.FileSystem.Extensions;
using Stac.Api.WebApi.Controllers.Extensions.Filter;
using Stac.Api.WebApi.Extensions;
using Stac.Api.WebApi.Implementations.Default.Extensions.Filter;
using Stac.Api.WebApi.Implementations.Shared.Exceptions;

// Create the default web application builder
var builder = WebApplication.CreateBuilder(args);

var configBuilder = new ConfigurationBuilder();

configBuilder.AddJsonFile("codegensettings.json", optional: true, reloadOnChange: true);
configBuilder.AddCommandLine(args);
configBuilder.AddEnvironmentVariables(prefix: "STACAPIFS_");
var configuration = configBuilder.Build();

// Configure the Todo repository and associated services
builder.Services.AddStacWebApi();
string catalogRootPath = configuration.GetValue<string>("CatalogRootPath") ?? Path.Combine(Path.GetTempPath(), "StacApi");
builder.Services.AddFileSystemData(builder =>
        builder.UseFileSystemProvider(catalogRootPath, true));

builder.Services.AddCodeGenOptions(configuration.GetSection("CodeGen"));


// Configure OpenAPI documentation for the Todo API
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddPolicy("All",
            builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyHeader();
            });
});

// Create the app
var app = builder.Build();

var prefix = configuration.GetValue<string>("PathBase", "/");

// Require use of HTTPS in production
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
    app.UseHttpsRedirection();
}

// Add Swagger endpoint for OpenAPI
app.UseOpenApi(app.Services.GetService<IOptions<CodeGenOptions>>().Value);
app.UseSwaggerUi3(c =>
{
    c.ConfigureSwaggerUi3(app.Services.GetService<IOptions<CodeGenOptions>>().Value);

}); // serve Swagger UI

app.UseReDoc(c =>
{
    c.Path = "/redoc";
    c.DocumentPath = "/openapi/v1.0.0-rc.2/core/openapi.yaml";
}); // serve ReDoc UI

app.UsePathBase(prefix);

app.UseRouting();
app.UseCors();

// global error handler
app.UseMiddleware<ErrorHandlerMiddleware>();

// Add the HTTP endpoints
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// Run the application
app.Run();

public partial class Program
{
    // Expose the Program class for use with WebApplicationFactory<T>
}

