using System.Net;
using System.Text.Json;
using FluentValidation;

namespace ServidoresAPI.Middleware;

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
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var errorResponse = new
        {
            Message = exception.Message,
            StatusCode = (int)HttpStatusCode.InternalServerError
        };

        switch (exception)
        {
            case ValidationException validationEx:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse = new
                {
                    Message = "Validation failed",
                    Errors = validationEx.Errors.Select(e => new
                    {
                        e.PropertyName,
                        e.ErrorMessage
                    }),
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
                break;

            case KeyNotFoundException:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse = new
                {
                    Message = "Resource not found",
                    StatusCode = (int)HttpStatusCode.NotFound
                };
                break;

            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        await response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    }
} 