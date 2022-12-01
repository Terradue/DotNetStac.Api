using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using Stac.Api.Clients.Core;

namespace Stac.Api.WebApi.Implementations.Shared.Exceptions
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IWebHostEnvironment env)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = System.Net.Mime.MediaTypeNames.Application.Json;

                switch (error)
                {
                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonConvert.SerializeObject(GetStacApiException(error, env.IsDevelopment()));
                await response.WriteAsync(result);
            }
        }

        private static ExceptionInfo GetStacApiException(Exception error, bool details)
        {
            ExceptionInfo exceptionInfo = new ExceptionInfo();
            exceptionInfo.Code = StatusCodes.Status500InternalServerError.ToString();
            exceptionInfo.Description = error.Message;
            if (details)
            {
                var lines = error.StackTrace?.Split("\n").Select(e => e.TrimStart());
                exceptionInfo.AdditionalProperties.Add("stackTrace", lines);
            }

            return exceptionInfo;
        }
    }
}