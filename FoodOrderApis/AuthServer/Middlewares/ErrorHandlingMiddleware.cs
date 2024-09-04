﻿using System.Text.Json;
namespace AuthServer.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            var errorResponse = new
            {
                error = ex.Message,
            };
            await HttpResponseWritingExtensions.WriteAsync(text: JsonSerializer.Serialize(errorResponse), response: context.Response);
        }
    }
}