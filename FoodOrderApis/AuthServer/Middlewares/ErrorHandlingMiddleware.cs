
using System.Net;
using System.Text.Json;
using AuthServer.Handler;
using FoodOrderApis.Common.Helpers;
using FoodOrderApis.Common.Validation;

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
        catch (ValidationException ex)
        {
            context.Response.StatusCode = ex.StatusCode;
            context.Response.ContentType = "application/json";
            if (ex.Errors.Count > 0)
            {
                var response = new ValidationErrorHandler
                {
                    statusCode = context.Response.StatusCode,
                    statusText = ex.Message,
                    errorMessage = ex.Message,
                    errors = ex.Errors
                };
                await context.Response.WriteAsync(response.ToString());
            }
        }
        catch (Exception ex)
        {
            var errorResponse = new ErrorResponseEx()
            {
                StatusCode = (int)ResponseStatusCode.InternalServerError,
                StatusText = "Internal Server Error",
                ErrorMessage = ex.Message
            };
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            await HttpResponseWritingExtensions.WriteAsync(text: JsonSerializer.Serialize(errorResponse), response: context.Response);
        }
    }
}