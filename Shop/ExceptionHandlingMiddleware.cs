﻿using System.Data;
using System.Text.Json;

namespace Shop
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (DuplicateNameException ex)
            {
                await context.Response.WriteAsync(GenerateErrorDetails(context, ex, StatusCodes.Status409Conflict));
            }
            catch (KeyNotFoundException ex)
            {
                await context.Response.WriteAsync(GenerateErrorDetails(context, ex, StatusCodes.Status404NotFound));
            }
            catch (FileLoadException ex)
            {
                await context.Response.WriteAsync(GenerateErrorDetails(context, ex, StatusCodes.Status400BadRequest));
            }
            catch (UnauthorizedAccessException ex)
            {
                await context.Response.WriteAsync(GenerateErrorDetails(context, ex, StatusCodes.Status401Unauthorized));
            }
            catch (ArgumentException ex)
            {
                await context.Response.WriteAsync(GenerateErrorDetails(context, ex, StatusCodes.Status400BadRequest));
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");

                await context.Response.WriteAsync(GenerateErrorDetails(context, ex, StatusCodes.Status500InternalServerError));
            }
        }

        private string GenerateErrorDetails(HttpContext context, Exception ex, int statusCode)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json+error";

            var errorDetails = new
            {
                Message = "O-o-ops...",
                Error = ex.Message,
            };

            return JsonSerializer.Serialize(errorDetails);
        }
    }

}
