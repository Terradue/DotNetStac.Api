// Copyright (c) Martin Costello, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.HttpOverrides;
using Stac.Api.WebApi.Extensions;

// Create the default web application builder
var builder = WebApplication.CreateBuilder(args);

// Configure the Todo repository and associated services
builder.Services.AddStacWebApi();

// Configure OpenAPI documentation for the Todo API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "STAC API", Version = "v1" });
});

// Create the app
var app = builder.Build();

// Require use of HTTPS in production
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
    app.UseHttpsRedirection();
}

// Add Swagger endpoint for OpenAPI
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "STAC API");
});

app.UseRouting();

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
