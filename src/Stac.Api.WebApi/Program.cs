// Copyright (c) Martin Costello, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.Options;
using Stac.Api.CodeGen;
using Stac.Api.WebApi.Extensions;
using Microsoft.Extensions.Configuration;
using NSwag.AspNetCore;
using Hellang.Middleware.ProblemDetails;

// Create the default web application builder
var builder = WebApplication.CreateBuilder(args);

var configBuilder = new ConfigurationBuilder();

configBuilder.AddJsonFile("codegensettings.json", optional: true, reloadOnChange: true);
configBuilder.AddCommandLine(args);
var configuration = configBuilder.Build();

// Configure the Todo repository and associated services
builder.Services.AddStacWebApi();
string catalogRootPath = configuration.GetValue<string>("catalogRootPath") ?? Path.Combine(Path.GetTempPath(), "StacApi");
builder.Services.AddFileSystemControllers(builder =>
        builder.UseFileSystemRoot(catalogRootPath, true));
        
builder.Services.AddCodeGenOptions(configuration.GetSection("CodeGen"));

builder.Services.AddProblemDetails(config =>
                StacWebApiExtensions.ConfigureProblemDetails(config, configuration, builder.Environment));


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

// Require use of HTTPS in production
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseProblemDetails();

// Add Swagger endpoint for OpenAPI
app.UseOpenApi(app.Services.GetService<IOptions<CodeGenOptions>>().Value);
app.UseSwaggerUi3(c =>
{
    c.ConfigureSwaggerUi3(app.Services.GetService<IOptions<CodeGenOptions>>().Value);

}); // serve Swagger UI

app.UseReDoc(c =>
{
    c.Path = "/redoc";
    c.DocumentPath = "/openapi/v1.0.0-rc.1/core/openapi.yaml";
}); // serve ReDoc UI

app.UseRouting();
app.UseCors();

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
